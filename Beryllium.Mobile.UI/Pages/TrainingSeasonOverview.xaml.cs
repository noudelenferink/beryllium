namespace Beryllium.Mobile.UI.Pages
{
   using Beryllium.Mobile.Core.ViewModels.Trainings;
   using MvvmCross.Forms.Presenters.Attributes;
   using MvvmCross.Forms.Views;
   using System;
   using Xamarin.Forms;
   using Xamarin.Forms.Xaml;

   [XamlCompilation(XamlCompilationOptions.Compile)]
   [MvxMasterDetailPagePresentation(Position = MasterDetailPosition.Detail, NoHistory = true, Title = "Seizoensoverzicht")]
   public partial class TrainingSeasonOverview : MvxContentPage<TrainingSeasonOverviewViewModel>
   {
      public TrainingSeasonOverview()
      {
         InitializeComponent();
      }
   }

   public class PositiveColorConverter : IValueConverter
   {
      public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
      {
         return (bool)value ? Color.Green : Color.LightGray;
      }

      public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
      {
         // You probably don't need this, this is used to convert the other way around
         // so from color to yes no or maybe
         throw new NotImplementedException();
      }
   }

   public class WidthCalculationConverter : IValueConverter
   {
      public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
      {
         if(!int.TryParse(parameter.ToString(), out var multiplier))
         {
            multiplier = 1;
         }

         return (int)value * multiplier;
      }

      public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
      {
         // You probably don't need this, this is used to convert the other way around
         // so from color to yes no or maybe
         throw new NotImplementedException();
      }
   }
}
