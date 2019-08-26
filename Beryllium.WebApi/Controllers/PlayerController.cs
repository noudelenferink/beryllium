namespace Beryllium.WebApi.Controllers
{
   using System;
   using System.Collections.Generic;
   using System.Linq;
   using System.Threading.Tasks;
   using Beryllium.Core;
   using Beryllium.Core.Players;
   using Beryllium.Core.Trainings;
   using Beryllium.EntityFrameworkCore;
   using Beryllium.Shared;
   using Beryllium.Shared.Players;
   using Beryllium.Shared.Trainings;
   using Microsoft.AspNetCore.Mvc;
   using Microsoft.Extensions.Logging;

   [Route("api/players")]
   public class PlayerController : BaseApiController<PlayerController>
   {
      private readonly IRepository<TeamPlayer> teamPlayerRepository;
      private readonly IRepository<Player> playerRepository;
      private readonly RankixContext context;

      public PlayerController(IRepository<TeamPlayer> teamPlayerRepository, IRepository<Player> playerRepository, RankixContext context)
      {
         this.teamPlayerRepository = teamPlayerRepository;
         this.playerRepository = playerRepository;
         this.context = context;
      }

      [HttpPost]
      [Route("GetTeamPlayers")]
      public IActionResult GetTeamPlayers(GetTrainingsInputDto input)
      {
         try
         {
            var teamPlayers = this.teamPlayerRepository.GetAll()
               .Where(tp => tp.SeasonID == input.SeasonId)
               .Where(tp => tp.TeamID == input.TeamId)
               .OrderBy(tp => tp.Player.LastName)
               .ThenBy(tp => tp.Player.FirstName)
               .Select(tp => new PlayerListDto
               {
                  PlayerId = tp.PlayerID,
                  FirstName = tp.Player.FirstName,
                  LastNamePrefix = tp.Player.LastNamePrefix,
                  LastName = tp.Player.LastName
               })
               .ToList();

            return this.Ok(new Result<List<PlayerListDto>>(teamPlayers));
         }
         catch (Exception ex)
         {
            this.Logger.LogError($"Failed to retrieve team players for season '{input.SeasonId}' and team '{input.TeamId}'", ex);
            return this.BadRequest(new Result<List<PlayerListDto>>($"Unable to retieve team players"));
         }
      }

      [HttpPost]
      [Route("FindPlayers")]
      public async Task<IActionResult> FindPlayers(SearchInputDto input)
      {
         var players = this.playerRepository.GetAll()
            .Where(p => 
               p.FirstName.Contains(input.SearchText, StringComparison.InvariantCultureIgnoreCase) 
               || p.LastName.Contains(input.SearchText, StringComparison.InvariantCultureIgnoreCase))
            .Select(p => new PlayerListDto
            {
               PlayerId = p.Id,
               FirstName = p.FirstName,
               LastNamePrefix = p.LastNamePrefix,
               LastName = p.LastName
            }).ToList();

         return this.Ok(new Result<List<PlayerListDto>>(players));
      }
   }
}