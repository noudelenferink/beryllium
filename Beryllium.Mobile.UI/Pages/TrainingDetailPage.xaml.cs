namespace Beryllium.Mobile.UI.Pages
{
   using Beryllium.Mobile.Core.ViewModels.Trainings;
   using MvvmCross.Forms.Presenters.Attributes;
   using MvvmCross.Forms.Views;
   using Xamarin.Forms;
   using Xamarin.Forms.Xaml;

   [XamlCompilation(XamlCompilationOptions.Compile)]
   [MvxMasterDetailPagePresentation(Position = MasterDetailPosition.Detail)]
   public partial class TrainingDetailPage : MvxContentPage<TrainingDetailViewModel>
   {
      public TrainingDetailPage()
      {
         InitializeComponent();
      }
   }
}