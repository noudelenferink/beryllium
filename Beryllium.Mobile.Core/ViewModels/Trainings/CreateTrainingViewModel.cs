namespace Beryllium.Mobile.Core.ViewModels.Trainings
{
   using Beryllium.Mobile.Core.Services;
   using MvvmCross.Commands;
   using MvvmCross.Logging;
   using MvvmCross.Navigation;
   using MvvmCross.ViewModels;
   using System;
   using System.Threading.Tasks;

   public class CreateTrainingViewModel : MvxNavigationViewModel
   {
      private readonly ITrainingService trainingService;
      private readonly IMvxNavigationService navigationService;

      public DateTime TrainingDate { get; set; } = DateTime.Now.Date;
      public bool IsBonus { get; set; }
      public IMvxAsyncCommand CreateCommand { get; private set; }
      public CreateTrainingViewModel(IMvxLogProvider logProvider, IMvxNavigationService navigationService, ITrainingService trainingService) : base(logProvider, navigationService)
      {
         this.trainingService = trainingService;
         this.navigationService = navigationService;
         CreateCommand = new MvxAsyncCommand(async () => await this.CreateTraining() );
      }

      private async Task CreateTraining()
      {
         await this.trainingService.CreateTraining(TrainingDate, IsBonus);
         await this.navigationService.Close(this);

      }
   }
}
