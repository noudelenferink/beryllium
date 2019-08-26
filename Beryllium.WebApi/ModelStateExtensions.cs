namespace Beryllium.WebApi
{
   using Microsoft.AspNetCore.Mvc.ModelBinding;
   using System.Collections.Generic;
   using System.Linq;

   public static class ModelStateExtensions
   {
      public static List<string> GetErrorMessages(this ModelStateDictionary dictionary)
      {
         return dictionary.SelectMany(m => m.Value.Errors)
                          .Select(m => m.ErrorMessage)
                          .ToList();
      }
   }
}
