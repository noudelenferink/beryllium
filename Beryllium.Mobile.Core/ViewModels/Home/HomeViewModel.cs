using Beryllium.Mobile.Core.Authentication;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Auth;
using Xamarin.Auth.Presenters;

namespace Beryllium.Mobile.Core.ViewModels.Home
{
   public class HomeViewModel : RankixBaseViewModel
   {
      public HomeViewModel()
      {
         var auth = new OAuth2Authenticator(
            Constants.AndroidClientId,
            null,
            Constants.Scope,
            new Uri(Constants.AuthorizeUrl),
            new Uri(Constants.AndroidRedirectUrl),
            new Uri(Constants.AccessTokenUrl),
            null,
            true
            );

         auth.Completed += OnAuthCompleted;
         auth.Error += OnAuthError;
         AuthenticationState.Authenticator = auth;
         
         var presenter = new OAuthLoginPresenter();
         presenter.Login(auth);
      }
      private string _userInfo;
      public string UserInfo
      {
         get => _userInfo;
         private set => SetProperty(ref _userInfo, value);
      }

      async void OnAuthCompleted(object sender, AuthenticatorCompletedEventArgs e)
      {
         var auth = sender as OAuth2Authenticator;
         if (e.IsAuthenticated)
         {
            var req = new OAuth2Request("GET", new Uri("https://www.googleapis.com/oauth2/v2/userinfo"), null, e.Account);
            var res = await req.GetResponseAsync();

            var result = await res.GetResponseTextAsync();
            this.UserInfo = result;
            Console.WriteLine(result);
         }
      }

      void OnAuthError(object sender, AuthenticatorErrorEventArgs e)
      {
         var auth = sender as OAuth2Authenticator;
         if (auth != null)
         {
            auth.Completed -= OnAuthCompleted;
            auth.Error -= OnAuthError;
         }

         Debug.WriteLine("Auth error: " + e.Message);
      }
   }
}
