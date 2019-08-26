using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Abp.ObjectMapping;
using Acr.UserDialogs;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;

namespace Beryllium.Mobile.Core.ViewModels
{
    public abstract class RankixBaseViewModel : MvxViewModel
    {
      private bool _isBusy;

      public IObjectMapper ObjectMapper { get; set; }

      public IMvxNavigationService NavigationService { get; set; }

      public bool IsNotBusy => !IsBusy;

      public bool IsBusy
      {
         get => _isBusy;
         set
         {
            _isBusy = value;
            RaisePropertyChanged(() => IsBusy);
            RaisePropertyChanged(() => IsNotBusy);
         }
      }

      public RankixBaseViewModel()
      {
         ObjectMapper = NullObjectMapper.Instance;
         //NavigationService = navigationService;
      }

      public virtual async Task InitializeAsync(object navigationData)
      {
         await Task.FromResult(false);
      }

      public object GetPropertyValue(string propertyName)
      {
         return GetType().GetProperty(propertyName).GetValue(this, null);
      }

      public T GetPropertyValue<T>(string propertyName)
      {
         var value = GetPropertyValue(propertyName);
         return (T)Convert.ChangeType(value, typeof(T));
      }

      public async Task SetBusyAsync(Func<Task> func, string loadingMessage = null)
      {
         if (loadingMessage == null)
         {
            loadingMessage = "Loading...";
         }

         UserDialogs.Instance.ShowLoading(loadingMessage, MaskType.None);
         IsBusy = true;

         try
         {
            await func();
         }
         catch(Exception ex)
         {
            UserDialogs.Instance.Alert("Er is iets misgegaan");
         }
         finally
         {
            UserDialogs.Instance.HideLoading();
            IsBusy = false;
         }
      }
   }
}
