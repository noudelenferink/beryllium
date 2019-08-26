namespace Beryllium.Mobile.UI.Pages
{
   using Beryllium.Mobile.Core.ViewModels.Trainings;
   using MvvmCross.Forms.Presenters.Attributes;
   using MvvmCross.Forms.Views;
   using Xamarin.Forms.Xaml;

   [XamlCompilation(XamlCompilationOptions.Compile)]
   [MvxMasterDetailPagePresentation(Position = MasterDetailPosition.Detail, NoHistory = true, Title = "Trainingbeheer")]
   public partial class TrainingListPage : MvxContentPage<TrainingsListViewModel>
   {
      public TrainingListPage()
      {
         InitializeComponent();
      }
   }
}
