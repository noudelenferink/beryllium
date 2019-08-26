namespace Beryllium.Mobile.UI.Pages
{
   using Beryllium.Mobile.Core.ViewModels.Trainings;
   using MvvmCross.Forms.Presenters.Attributes;
   using MvvmCross.Forms.Views;
   using Xamarin.Forms.Xaml;

   [XamlCompilation(XamlCompilationOptions.Compile)]
   [MvxModalPresentation(WrapInNavigationPage = true)]
   public partial class CreateTrainingPage : MvxContentPage<CreateTrainingViewModel>
   {
      public CreateTrainingPage()
      {
         InitializeComponent();
      }
   }
}