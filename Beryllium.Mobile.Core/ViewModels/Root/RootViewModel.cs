using System;
using System.Collections.Generic;
using System.Text;
using MvvmCross.Navigation;
using Beryllium.Mobile.Core.ViewModels.Home;
using Beryllium.Mobile.Core.ViewModels.Menu;
using Beryllium.Mobile.Core.ViewModels.Trainings;
using System.Threading.Tasks;
using MvvmCross.ViewModels;
using Xamarin.Forms;

namespace Beryllium.Mobile.Core.ViewModels.Root
{
   public class RootViewModel : RankixBaseViewModel
   {
      readonly IMvxNavigationService _navigationService;

      public bool AppLoaded { get; set; }

      public RootViewModel(IMvxNavigationService navigationService)
      {
         _navigationService = navigationService;
      }

      public override void ViewAppearing()
      {
         base.ViewAppearing();
         MvxNotifyTask.Create(async () => await this.InitializeApplication());
      }

      private async Task InitializeApplication()
      {
         await this.LoadSession();

         await this.InitializeViewModels();
      }

      public async Task InitializeViewModels()
      {
         if (!this.AppLoaded)
         {
            await _navigationService.Navigate<MenuViewModel>();
            await _navigationService.Navigate<TrainingSeasonOverviewViewModel>();

            this.AppLoaded = true;
         }
      }

      private async Task LoadSession()
      {
         var api = new RankixApi();
         var userInfo = await api.GetCurrentUserInformation();
         Application.Current.Properties.Add("userInfo", userInfo);
      }
   }
}
