namespace Beryllium.Mobile.Core.Authentication
{
   using System.Collections.Generic;

   public class User
    {
        public string Name { get; set; }
        public List<string> Permissions { get; set; }
    }
}
