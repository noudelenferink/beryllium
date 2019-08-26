using Beryllium.Shared.Players;
using Beryllium.Shared.Trainings;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Beryllium.Mobile.Core.Services
{
   public interface ITrainingService
   {
      Task<List<TrainingListDto>> GetTrainingsAsync(int seasonId, int teamId);

      Task<TrainingDetailDto> GetTrainingAsync(int trainingId);

      Task<bool> UpdateTrainingAsync(UpdateTrainingDto training);

      Task<List<TrainingSeasonOverviewDto>> GetTrainingSeasonOverview(int seasonId, int teamId);

      Task<List<PlayerListDto>> GetTeamPlayers(int seasonId, int teamId);

      Task CreateTraining(DateTime trainingDate, bool isBonus);
      Task<bool> DeleteTraining(int trainingId);
   }
}
