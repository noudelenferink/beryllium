namespace Beryllium.WebApi.Controllers
{
   using System;
   using System.Collections.Generic;
   using System.Linq;
   using System.Threading.Tasks;
   using Beryllium.Core;
   using Beryllium.Core.Trainings;
   using Beryllium.EntityFrameworkCore;
   using Beryllium.Shared;
   using Beryllium.Shared.Players;
   using Beryllium.Shared.Trainings;
   using Microsoft.AspNetCore.Mvc;
   using Microsoft.Extensions.Logging;

   [Route("api/trainings")]
   public class TrainingController : BaseApiController<TrainingController>
   {
      private readonly IRepository<Training> trainingRepository;
      private readonly RankixContext context;

      public TrainingController(IRepository<Training> trainingRepository, RankixContext context)
      {
         this.trainingRepository = trainingRepository;
         this.context = context;
      }

      [HttpPost]
      [Route("GetTrainings")]
      public IActionResult GetTrainings(GetTrainingsInputDto input)
      {
         this.Logger.LogInformation($"Retrieve trainings for season '{input.SeasonId}' and team '{input.TeamId}'");
         try
         {
            var trainings = this.trainingRepository.GetAll()
               .Where(t => t.SeasonID == input.SeasonId)
               .Where(t => t.TeamID == input.TeamId)
               .Where(t => t.IsBonus || !input.IncludeBonus)
               .Select(t => new TrainingListDto
               {
                  Id = t.Id,
                  Date = t.TrainingDate,
                  IsBonus = t.IsBonus,
                  NumAttendees = t.TrainingPlayers.Count
               })
               .OrderBy(t => t.Date)
               .ToList();

            return this.Ok(new Result<List<TrainingListDto>>(trainings));
         }
         catch (Exception ex)
         {
            this.Logger.LogError($"Failed to retrieve trainings for season '{input.SeasonId}' and team '{input.TeamId}'", ex);
            return this.BadRequest(new Result<List<TrainingListDto>>($"Unable to retieve trainings"));
         }
      }

      [HttpPost]
      [Route("GetTraining")]
      public IActionResult GetTraining(EntityInputDto input)
      {
         this.Logger.LogInformation($"Retrieving training '{input.Id}'");

         // TODO - Move to service?
         var training = this.trainingRepository.GetById(input.Id);

         if (training == null)
         {
            //
            return BadRequest(new Result<TrainingDetailDto>($"Could not find training '{input.Id}'"));
         }

         var result = new TrainingDetailDto
         {
            Id = training.Id,
            SeasonId = training.SeasonID,
            TeamId = training.TeamID,
            Date = training.TrainingDate,
            IsBonus = training.IsBonus,
            Attendees = training.TrainingPlayers
               .OrderBy(tp => tp.Player.LastName)
               .ThenBy(tp => tp.Player.FirstName)
               .Select(tp => new PlayerListDto
               {
                  PlayerId = tp.PlayerID,
                  FirstName = tp.Player.FirstName,
                  LastNamePrefix = tp.Player.LastNamePrefix,
                  LastName = tp.Player.LastName
               })
            .ToList()
         };

         return Ok(new Result<TrainingDetailDto>(result));
      }

      [HttpPost]
      [Route("CreateTraining")]
      public async Task<IActionResult> CreateTraining(CreateTrainingDto input)
      {
         this.Logger.LogInformation($"Create new training for season '{input.SeasonId}' and team '{input.TeamId}' on date '{input.TrainingDate}'");
         var training = new Training
         {
            TeamID = input.TeamId,
            SeasonID = input.SeasonId,
            IsBonus = input.IsBonus,
            TrainingDate = input.TrainingDate
         };

         await this.trainingRepository.Create(training);

         return Ok(true);
      }

      [HttpPost]
      [Route("UpdateTraining")]
      public async Task<IActionResult> UpdateTraining(UpdateTrainingDto input)
      {
         this.Logger.LogInformation($"Update training '{input.Id}'");
         var training = this.trainingRepository.GetById(input.Id);
         if (training == null)
         {
            // Training not found
            // TODO - Log and return error
            return null;
         }

         training.IsBonus = input.IsBonus;

         // Date already is in UTC, but is converted to UTC again, so set back to local one time.
         training.TrainingDate = input.TrainingDate.ToLocalTime();

         var current = training.TrainingPlayers.Select(tp => tp.PlayerID);

         // Add new
         var attendeesToAdd = input.Attendees.Except(current).ToList();
         foreach (var newAttendee in attendeesToAdd)
         {
            training.TrainingPlayers.Add(new PlayerTraining { TrainingID = training.Id, PlayerID = newAttendee });
         };

         // Remove old
         var attendeesToRemove = current.Except(input.Attendees).ToList();
         foreach (var oldAttendee in attendeesToRemove)
         {
            training.TrainingPlayers.RemoveAll(tp => tp.PlayerID == oldAttendee);
         }

         var result = await this.trainingRepository.UpdateAsync(training);

         return Ok(true);
      }

      [HttpPost]
      [Route("GetTrainingSeasonOverview")]
      public IActionResult GetTrainingSeasonOverview(GetTrainingSeasonOverviewInputDto input)
      {
         this.Logger.LogInformation($"Get training season overview for season '{input.SeasonId}' and team '{input.TeamId}'");
         var teamTrainings = this.context.Trainings
            .Where(t => t.SeasonID == input.SeasonId)
            .Where(t => t.TeamID == input.TeamId)
            .ToList();

         var hasAttended = this.context.Trainings
            .Where(t => t.SeasonID == input.SeasonId)
            .Where(t => t.TeamID == input.TeamId)
            .Where(t => !t.IsBonus)
            .SelectMany(t => t.TrainingPlayers)
            .GroupBy(tp => tp.Player)
            .Select(p => new
            {
               PlayerId = p.Key.Id,
               HasAttended = p.Count(),
               BonusAttended = p.Where(y => y.Training.IsBonus).Count()
            })
            .ToList();

         var rawResult = this.context.TeamPlayers
            .Where(tp => tp.SeasonID == input.SeasonId)
            .Where(tp => tp.TeamID == input.TeamId)
            .Select(tp => new
            {
               PlayerId = tp.Player.Id,
               tp.Player.FirstName,
               tp.Player.LastName,
               tp.Player.LastNamePrefix,
               HasAttended = hasAttended
                              .Where(a => a.PlayerId == tp.PlayerID)
                              .Select(a => a.HasAttended)
                              .DefaultIfEmpty()
                              .Single(),
               CouldAttended = teamTrainings
                                 .Where(tt => !tt.IsBonus)
                                 .Where(tt => tt.TrainingDate >= tp.EffectiveDate)
                                 .Where(tt => tt.TrainingDate < tp.ExpiryDate || !tp.ExpiryDate.HasValue)
                                 .Count(),
               BonusAttended = hasAttended
                                 .Where(a => a.PlayerId == tp.PlayerID)
                                 .Select(a => a.BonusAttended)
                                 .DefaultIfEmpty()
                                 .Single(),
               Recent = teamTrainings
                           .Where(tt => !tt.IsBonus)
                           .Select(tt => new
                           {
                              tt.TrainingDate,
                              HasAttended = tt.TrainingPlayers.Any(x => x.PlayerID == tp.PlayerID)
                           })
                           .OrderByDescending(tt => tt.TrainingDate)
                           .Take(5)
                           .OrderBy(tt => tt.TrainingDate)
                           .ToDictionary(
                              x => x.TrainingDate.ToString("yyyy-MM-dd"),
                              x => x.HasAttended
                           )
            })
            .ToList();

         var result = rawResult
            .Select(r => new TrainingSeasonOverviewDto
            {
               Player = new PlayerListDto
               {
                  PlayerId = r.PlayerId,
                  FirstName = r.FirstName,
                  LastNamePrefix = r.LastNamePrefix,
                  LastName = r.LastName,
               },
               AttendancePercentage = r.CouldAttended > 0 ? decimal.Divide(r.HasAttended, r.CouldAttended) * 100 : 0,
               BonusAttended = r.BonusAttended,
               Recent = r.Recent
            });

         result = result.OrderByDescending(r => r.AttendancePercentage)
                        .ThenByDescending(r => r.BonusAttended)
                        .ThenBy(r => r.Player.LastName)
                        .ThenBy(r => r.Player.FirstName)
                        .ToList();

         return Ok(result);
      }

      [HttpPost]
      [Route("DeleteTraining")]
      public async Task<IActionResult> DeleteTraining(EntityInputDto input)
      {
         this.Logger.LogInformation($"Delete training '{input.Id}'");
         try
         {
            await this.trainingRepository.Delete(input.Id);
            return this.Ok(new Result());
         }
         catch(Exception ex)
         {
            this.Logger.LogError($"Failed to delete training '{input.Id}'. Exception: {ex}");
            return this.BadRequest(new Result("Failed to delete training"));
         }

      }
   }
}