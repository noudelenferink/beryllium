namespace Beryllium.Mobile.Core
{
   using Acr.UserDialogs;
   using Beryllium.Shared;
   using Beryllium.Shared.Players;
   using Beryllium.Shared.Session;
   using Beryllium.Shared.Trainings;
   using RestSharp;
   using System;
   using System.Collections.Generic;
   using System.Threading.Tasks;
   using Xamarin.Essentials;

   public class RankixApi
   {
#if DEBUG
      private const string BaseUrl = "http://beryllium.local";
#else
      private const string BaseUrl = "http://rankix-dev.nifnic.nl:81";
#endif

      private readonly IRestClient client;

      public RankixApi()
      {
         this.client = new RestClient(BaseUrl);
         Initialize();
      }

      private async void Initialize()
      {
         var accessToken = await SecureStorage.GetAsync(SecureStorageKeys.AccessToken);
         this.client.Timeout = 10 * 1000; // 10 seconds
         this.client.AddDefaultHeader("Content-Type", "application/json");
         this.client.AddDefaultHeader("Authorization", $"Bearer {accessToken}");
      }

      public async Task<T> ExecuteAsync<T>(RestRequest request) where T : new()
      {
         var response = await this.client.ExecuteTaskAsync<T>(request);

         if (response.ErrorException != null)
         {
            const string message = "Error retrieving response.  Check inner details for more info.";
            var exception = new ApplicationException(message, response.ErrorException);
            Console.WriteLine(exception);
            throw exception;
         }

         return response.Data;
      }

      public async Task<CurrentUserInformation> GetCurrentUserInformation()
      {
         var request = new RestRequest("/api/session/GetCurrentUserInformation", Method.POST, DataFormat.Json);
         var result = await this.ExecuteAsync<CurrentUserInformation>(request);
         return result;

      }

      public async Task<Result<List<TrainingListDto>>> GetTrainings(int seasonId, int teamId)
      {
         var request = new RestRequest("/api/trainings/GetTrainings", Method.POST, DataFormat.Json);
         request.AddJsonBody(new GetTrainingsInputDto { SeasonId = seasonId, TeamId = teamId });
         var result = await this.ExecuteAsync<Result<List<TrainingListDto>>>(request);
         return result;

      }

      public async Task<Result<TrainingDetailDto>> GetTraining(int trainingId)
      {
         var request = new RestRequest($"/api/trainings/GetTraining", Method.POST, DataFormat.Json);
         request.AddJsonBody(new EntityInputDto { Id = trainingId });
         var result = await this.ExecuteAsync<Result<TrainingDetailDto>>(request);
         return result;
      }

      public async Task<bool> UpdateTraining(UpdateTrainingDto trainingToUpdate)
      {
         var request = new RestRequest($"/api/trainings/UpdateTraining", Method.POST, DataFormat.Json);
         request.AddJsonBody(trainingToUpdate);

         var result = await this.ExecuteAsync<bool>(request);
         return result;
      }

      public async Task<List<TrainingSeasonOverviewDto>> GetTrainingSeasonOverview(int seasonId, int teamId)
      {
         var request = new RestRequest($"/api/trainings/GetTrainingSeasonOverview", Method.POST, DataFormat.Json);
         request.AddJsonBody(new GetTrainingSeasonOverviewInputDto { SeasonId = seasonId, TeamId = teamId });

         var result = await this.ExecuteAsync<List<TrainingSeasonOverviewDto>>(request);
         return result;
      }

      public async Task<Result<List<PlayerListDto>>> GetTeamPlayers(int seasonId, int teamId)
      {
         var request = new RestRequest($"/api/players/GetTeamPlayers", Method.POST, DataFormat.Json);
         request.AddJsonBody(new GetTrainingSeasonOverviewInputDto { SeasonId = seasonId, TeamId = teamId });

         var result = await this.ExecuteAsync<Result<List<PlayerListDto>>>(request);
         return result;
      }

      public async Task<bool> CreateTraining(int seasonId, int teamId, DateTime trainingDate, bool isBonus)
      {
         var request = new RestRequest($"/api/trainings/CreateTraining", Method.POST, DataFormat.Json);
         request.AddJsonBody(new CreateTrainingDto { SeasonId = seasonId, TeamId = teamId , TrainingDate = trainingDate, IsBonus = isBonus});

         var result = await this.ExecuteAsync<bool>(request);
         return result;
      }

      public async Task<Result> DeleteTraining(int trainingId)
      {
         var request = new RestRequest($"/api/trainings/DeleteTraining", Method.POST, DataFormat.Json);
         request.AddJsonBody(new EntityInputDto { Id = trainingId });

         var result = await this.ExecuteAsync<Result>(request);
         return result;
      }

      public async Task<Result<List<PlayerListDto>>> FindPlayers(string searchText)
      {
         var request = new RestRequest($"/api/players/FindPlayers", Method.POST, DataFormat.Json);
         request.AddJsonBody(new SearchInputDto { SearchText = searchText });

         var result = await this.ExecuteAsync<Result<List<PlayerListDto>>>(request);
         return result;
      }
   }
}
