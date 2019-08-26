using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;

namespace Beryllium.Mobile.UI
{
   public partial class App : Application
   {
      public App()
      {
         //Register Syncfusion license
         Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("MTM1NTk4QDMxMzcyZTMyMmUzMExwSVlFSkRzSXYrNmJ2eWNYWThCSVJXNFNXTVp3eElQNllsUG9pblQ2d3c9");

         InitializeComponent();
      }
   }
}
