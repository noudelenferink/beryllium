namespace Beryllium.Mobile.Core.ViewModels.Trainings
{
   using Acr.UserDialogs;
   using Beryllium.Mobile.Core.Services;
   using Beryllium.Shared.Trainings;
   using MvvmCross.Commands;
   using MvvmCross.Navigation;
   using MvvmCross.Presenters;
   using MvvmHelpers;
   using System.Linq;
   using System.Threading.Tasks;

   public class TrainingsListViewModel : RankixBaseViewModel
   {
      private readonly ITrainingService trainingService;
      private readonly IMvxNavigationService navigationService;
      private readonly IMvxViewPresenter viewPresenter;

      public IMvxAsyncCommand ShowTrainingDetailPageAsyncCommand { get; private set; }

      public IMvxAsyncCommand LoadAsyncCommand { get; set; }
      public IMvxAsyncCommand CreateTrainingCommand { get; set; }
      public IMvxAsyncCommand<int> DeleteTrainingCommand { get; set; }


      public TrainingsListViewModel(ITrainingService trainingService, IMvxNavigationService navigationService, IMvxViewPresenter viewPresenter)
      {
         this.trainingService = trainingService;
         this.navigationService = navigationService;
         this.viewPresenter = viewPresenter;
         ShowTrainingDetailPageAsyncCommand = new MvxAsyncCommand(ShowTrainingDetailPageAsync);
         LoadAsyncCommand = new MvxAsyncCommand(async () => await LoadTrainings());
         CreateTrainingCommand = new MvxAsyncCommand(async () => await this.CreateTraining() );
         DeleteTrainingCommand = new MvxAsyncCommand<int>(async (x) => await DeleteTrainingAsync(x));
      }

      public async Task LoadTrainings()
      {
         await SetBusyAsync(async () =>
         {
            var result = await this.trainingService.GetTrainingsAsync(this.CurrentUserInformation.DefaultSeason.Id, this.CurrentUserInformation.DefaultTeam.Id);
            Trainings.ReplaceRange(result.OrderByDescending(t => t.Date));
         });
      }

      public async Task ShowTrainingDetailPageAsync()
      {
         await this.navigationService.Navigate<TrainingDetailViewModel, DetailNavigationArgs>(new DetailNavigationArgs { Id = SelectedTraining.Id });
      }

      public async Task DeleteTrainingAsync(int trainingId)
      {
         var confirmed = await UserDialogs.Instance.ConfirmAsync(new ConfirmConfig { Message = $"Verwijderen?" });
         if (confirmed)
         {
            var success = await this.trainingService.DeleteTraining(trainingId);
            UserDialogs.Instance.Toast(new ToastConfig($"Verwijderen {(success ? "gelukt" : "mislukt")}"));
            await this.LoadTrainings();
         }
      }

      public async Task CreateTraining()
      {
         var result = await this.navigationService.Navigate<CreateTrainingViewModel>();
         //if(result)
         //{
         //   await this.LoadTrainings();
         //}
      }

      private TrainingListDto _selectedTraining;

      public TrainingListDto SelectedTraining
      {
         get => _selectedTraining;
         set => SetProperty(ref _selectedTraining, value);
      }

      private ObservableRangeCollection<TrainingListDto> _trainings = new ObservableRangeCollection<TrainingListDto>();

      public ObservableRangeCollection<TrainingListDto> Trainings
      {
         get => _trainings;
         set
         {
            _trainings = value;
            RaisePropertyChanged(() => Trainings);
         }
      }
   }
}
