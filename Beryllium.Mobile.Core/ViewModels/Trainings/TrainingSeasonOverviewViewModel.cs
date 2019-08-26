namespace Beryllium.Mobile.Core.ViewModels.Trainings
{
   using Beryllium.Mobile.Core.Services;
   using Beryllium.Shared.Trainings;
   using MvvmCross.Commands;
   using MvvmCross.Navigation;
   using MvvmHelpers;
   using System.Threading.Tasks;

   public class TrainingSeasonOverviewViewModel : RankixBaseViewModel
   {
      private ITrainingService trainingService;
      private IMvxNavigationService navigationService;
      private ObservableRangeCollection<TrainingSeasonOverviewDto> _seasonOverview = new ObservableRangeCollection<TrainingSeasonOverviewDto>();

      public IMvxAsyncCommand LoadAsyncCommand { get; set; }

      public ObservableRangeCollection<TrainingSeasonOverviewDto> SeasonOverview
      {
         get => _seasonOverview;
         set
         {
            _seasonOverview = value;
            RaisePropertyChanged(() => SeasonOverview);
         }
      }

      public TrainingSeasonOverviewViewModel(ITrainingService trainingService, IMvxNavigationService navigationService)
      {
         this.trainingService = trainingService;
         this.navigationService = navigationService;
         LoadAsyncCommand = new MvxAsyncCommand(async () => await LoadSeasonOverview());
      }

      private async Task LoadSeasonOverview()
      {
         await SetBusyAsync(async () =>
         {
            var result = await this.trainingService.GetTrainingSeasonOverview(6, 27);
            SeasonOverview.ReplaceRange(result);
         });
      }
   }

}
