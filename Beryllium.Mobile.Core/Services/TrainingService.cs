﻿namespace Beryllium.Mobile.Core.Services
{
   using Beryllium.Shared.Trainings;
   using System.Collections.Generic;
   using System.Threading.Tasks;
   using System;
   using Beryllium.Shared.Players;
   using System.Linq;
   using Beryllium.Shared.Session;
   using Xamarin.Forms;

   public class TrainingService : ITrainingService
   {
      public RankixApi RankixApi { get; set; }
      public CurrentUserInformation CurrentUserInformation { get; set; }
      public int CurrentTeamId { get; set; }

      public TrainingService()
      {
         this.RankixApi = new RankixApi();
         if(!Application.Current.Properties.TryGetValue("userInfo", out var userInfoProp))
         {
            // Error
         }

         this.CurrentUserInformation = (CurrentUserInformation)userInfoProp;
      }

      public async Task<List<TrainingListDto>> GetTrainingsAsync(int seasonId, int teamId)
      {
         var result = await this.RankixApi.GetTrainings(seasonId, teamId);

         if (!result.Success)
         {
            // Error handling
            return null;
         }

         return result.Data;
      }

      public async Task<TrainingDetailDto> GetTrainingAsync(int trainingId)
      {
         var result = await this.RankixApi.GetTraining(trainingId);

         if (!result.Success)
         {
            // Error handling
            return null;
         }

         return result.Data;
      }

      public async Task<TrainingDetailDto> GetTrainingForEditAsync(int trainingId)
      {
         var result = await this.RankixApi.GetTraining(trainingId);
         
         if (!result.Success)
         {
            // Error handling
            return null;
         }
         return result.Data;
      }

      public async Task<bool> UpdateTrainingAsync(UpdateTrainingDto training)
      {
         DateTime.SpecifyKind(training.TrainingDate, DateTimeKind.Utc);
         return await this.RankixApi.UpdateTraining(training);
      }

      public async Task<List<TrainingSeasonOverviewDto>> GetTrainingSeasonOverview(int seasonId, int teamId)
      {
         return await this.RankixApi.GetTrainingSeasonOverview(seasonId, teamId);
      }

      public async Task<List<PlayerListDto>> GetTeamPlayers(int seasonId, int teamId)
      {
         var result = await this.RankixApi.GetTeamPlayers(seasonId, teamId);

         if (!result.Success)
         {
            // Error handling
            return null;
         }

         return result.Data;
      }

      public async Task CreateTraining(DateTime trainingDate, bool isBonus)
      {
         await this.RankixApi.CreateTraining(this.CurrentUserInformation.DefaultSeason.Id, this.CurrentUserInformation.DefaultTeam.Id, trainingDate, isBonus);
      }

      public async Task<bool> DeleteTraining(int trainingId)
      {
         var result = await this.RankixApi.DeleteTraining(trainingId);
         return result.Success;
      }
   }
}
