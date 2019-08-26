namespace Beryllium.Mobile.Core.ViewModels.Menu
{
   using System.Collections.ObjectModel;
   using System.Threading.Tasks;
   using MvvmCross.Commands;
   using MvvmCross.Navigation;
   using MvvmCross.ViewModels;
   using Beryllium.Mobile.Core.ViewModels.Home;
   using Xamarin.Forms;
   using Beryllium.Mobile.Core.ViewModels.Trainings;
   using MvvmHelpers;
   using System.ComponentModel;
   using System;

   public class MenuViewModel : RankixBaseViewModel
   {
      readonly IMvxNavigationService _navigationService;

      public IMvxAsyncCommand ShowDetailPageAsyncCommand { get; private set; }

      public MenuViewModel(IMvxNavigationService navigationService)
      {
         _navigationService = navigationService;
         MenuItemList = new MvxObservableCollection<MenuItem>()
            {
            new MenuItem{ Key = "Home", Title = "Home" , SortOrder = 1, TargetViewModel = typeof(HomeViewModel)},
            new MenuItem{ Key = "TrainingSeasonOverview", Title = "Training seizoensoverzicht" , SortOrder = 2, TargetViewModel = typeof(TrainingSeasonOverviewViewModel)},
            new MenuItem{ Key = "TrainingManager", Title = "Trainingbeheer" , SortOrder = 3, TargetViewModel = typeof(TrainingsListViewModel)}
            };

         ShowDetailPageAsyncCommand = new MvxAsyncCommand(ShowDetailPageAsync);
      }

      private ObservableCollection<MenuItem> _menuItemList;
      public ObservableCollection<MenuItem> MenuItemList
      {
         get => _menuItemList;
         set => SetProperty(ref _menuItemList, value);
      }

      private async Task ShowDetailPageAsync()
      {
         // Implement your logic here.
         await _navigationService.Navigate(SelectedMenuItem.TargetViewModel);

         if (Application.Current.MainPage is MasterDetailPage masterDetailPage)
         {
            masterDetailPage.IsPresented = false;
         }
         else if (Application.Current.MainPage is NavigationPage navigationPage
                  && navigationPage.CurrentPage is MasterDetailPage nestedMasterDetail)
         {
            nestedMasterDetail.IsPresented = false;
         }
      }

      private MenuItem _selectedMenuItem;
      public MenuItem SelectedMenuItem
      {
         get => _selectedMenuItem;
         set => SetProperty(ref _selectedMenuItem, value);
      }
   }

   public class MenuItem
   {
      public string Key { get; set; }
      public string Title { get; set; }
      public int SortOrder { get; set; }
      public Type TargetViewModel { get; set; }
   }
}
