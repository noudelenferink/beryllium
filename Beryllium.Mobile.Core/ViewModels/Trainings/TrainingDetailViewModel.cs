namespace Beryllium.Mobile.Core.ViewModels.Trainings
{
   using Acr.UserDialogs;
   using Beryllium.Mobile.Core.Services;
   using Beryllium.Shared.Players;
   using Beryllium.Shared.Trainings;
   using MvvmCross.Commands;
   using MvvmCross.Navigation;
   using MvvmHelpers;
   using System;
   using System.Collections.Generic;
   using System.Linq;
   using System.Threading.Tasks;

   public class TrainingDetailViewModel : RankixBaseViewModel<DetailNavigationArgs>
   {
      private readonly ITrainingService trainingService;
      private readonly IMvxNavigationService navigationService;
      private TrainingDetailDto _training;
      private DetailNavigationArgs currentParameters;

      public IMvxAsyncCommand SaveTrainingCommand { get; set; }
      public IMvxAsyncCommand AddPlayerCommand { get; set; }

      public TrainingDetailDto Training
      {
         get => _training;
         private set => SetProperty(ref _training, value);
      }

      private ObservableRangeCollection<AttendeeInfoItem> _attendeeInfo;

      public ObservableRangeCollection<AttendeeInfoItem> AttendeeInfo
      {
         get => _attendeeInfo;
         private set => SetProperty(ref _attendeeInfo, value);
      }

      public TrainingDetailViewModel(ITrainingService trainingService, IMvxNavigationService navigationService)
      {
         this.trainingService = trainingService;
         this.navigationService = navigationService;
         SaveTrainingCommand = new MvxAsyncCommand(async () => await SaveTrainingAsync());
         AddPlayerCommand = new MvxAsyncCommand(async () => await this.ShowAddPlayerModalAsync());
      }

      public override void Prepare(DetailNavigationArgs parameters)
      {
         this.currentParameters = parameters;
      }

      public override async Task Initialize()
      {
         await base.Initialize();
         this.Training = await this.trainingService.GetTrainingAsync(this.currentParameters.Id);

         var teamPlayers = await this.trainingService.GetTeamPlayers(this.Training.SeasonId, this.Training.TeamId);

         // First, set the attendee info based on all the team players and whether they are marked as a attendee for the given training.
         this.AttendeeInfo = new ObservableRangeCollection<AttendeeInfoItem>();
         this.AttendeeInfo.AddRange(teamPlayers.Select(tp => new AttendeeInfoItem(tp, this.Training.Attendees.Any(a => a.PlayerId == tp.PlayerId))));

         // After that, add the attendees that are not part of the team.
         this.AttendeeInfo.AddRange(this.Training.Attendees.Where(a => !teamPlayers.Select(tp => tp.PlayerId).Contains(a.PlayerId)).Select(a => new AttendeeInfoItem(a, true)));
      }

      public async Task ShowAddPlayerModalAsync()
      {
         var currentPlayerIds = this.AttendeeInfo.Select(ai => ai.Player.PlayerId).ToArray();
         var result = await this.navigationService.Navigate<TrainingAddPlayerViewModel, int[], PlayerListDto>(currentPlayerIds);

         if (result != null)
         {
            UserDialogs.Instance.Alert(result.FullName);
            this.AttendeeInfo.Add(item: new AttendeeInfoItem(result, true));
         }
      }

      public async Task SaveTrainingAsync()
      {
         var updateData = new UpdateTrainingDto
         {
            Id = Training.Id,
            TrainingDate = Training.Date,
            IsBonus = Training.IsBonus,
            Attendees = this.AttendeeInfo
                           .Where(ai => ai.HasAttended)
                           .Select(ai => ai.Player.PlayerId)
                           .ToList()

         };

         await this.trainingService.UpdateTrainingAsync(updateData);
         await this.navigationService.Close(this);
      }
   }

   public class AttendeeInfoItem
   {
      public AttendeeInfoItem(PlayerListDto player, bool hasAttended)
      {
         this.Player = player;
         this.HasAttended = hasAttended;
      }

      public PlayerListDto Player { get; set; }
      public bool HasAttended { get; set; }
   }
}
