namespace Beryllium.Mobile.Droid
{
   using Android.App;
   using Android.OS;
   using MvvmCross.Forms.Platforms.Android.Views;
   using Beryllium.Mobile.Core.ViewModels.Main;
   using Acr.UserDialogs;
   using MvvmCross;
   using MvvmCross.Platforms.Android;
   using Xamarin.Forms;
   using Android.Content.PM;
   using Android.Content;
   using Auth0.OidcClient;

   [Activity(
        Theme = "@style/AppTheme", LaunchMode = LaunchMode.SingleTask)]
   [IntentFilter(
        new[] { Intent.ActionView },
        Categories = new[] { Intent.CategoryDefault, Intent.CategoryBrowsable },
        DataScheme = "com.nifnic.rankix.dev",
        DataHost = "rankix-dev.eu.auth0.com",
        DataPathPrefix = "/android/com.nifnic.rankix.dev/callback")]
   public class MainActivity : MvxFormsAppCompatActivity<MainViewModel>
   {
      public override void InitializeApplication()
      {
         base.InitializeApplication();
      }

      protected override void OnCreate(Bundle bundle)
      {
         TabLayoutResource = Resource.Layout.Tabbar;
         ToolbarResource = Resource.Layout.Toolbar;
         UserDialogs.Init(() => Mvx.IoCProvider.Resolve<IMvxAndroidCurrentTopActivity>().Activity);
         base.OnCreate(bundle);

      }

      protected override void OnNewIntent(Intent intent)
      {
         base.OnNewIntent(intent);

         ActivityMediator.Instance.Send(intent.DataString);
      }
   }
}
