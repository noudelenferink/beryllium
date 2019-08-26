namespace Beryllium.Mobile.UI.Pages
{
   using Beryllium.Mobile.Core.ViewModels.Trainings;
   using Beryllium.Shared.Players;
   using MvvmCross.Forms.Presenters.Attributes;
   using MvvmCross.Forms.Views;
   using Syncfusion.SfAutoComplete.XForms;
   using System.Threading;
   using Xamarin.Forms.Xaml;

   [XamlCompilation(XamlCompilationOptions.Compile)]
   [MvxModalPresentation(WrapInNavigationPage = true)]
   public partial class TrainingAddPlayerPage : MvxContentPage<TrainingAddPlayerViewModel>
   {
      public TrainingAddPlayerPage()
      {
         InitializeComponent();
      }

      private void AutoComplete_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
      {
         var value = (e?.Value as PlayerListDto);
         if(value == null)
         {
            return;
         }

         this.ViewModel.SelectedPlayer = value;
      }

      private void AutoComplete_OnValueChanged(object sender, ValueChangedEventArgs e)
      {
         var text = e?.Value;
         if (string.IsNullOrEmpty(text)) return;

         this.ViewModel?.RefreshAutoCompleteDataSourceItems(text);
      }
   }
}