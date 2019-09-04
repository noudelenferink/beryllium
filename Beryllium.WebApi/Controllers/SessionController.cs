using Beryllium.Shared;
using Beryllium.Shared.Session;
using Microsoft.AspNetCore.Mvc;

namespace Beryllium.WebApi.Controllers
{
   [Route("api/session")]
   public class SessionController : BaseApiController
   {
      [HttpPost]
      [Route("GetCurrentUserInformation")]
      public IActionResult GetCurrentUserInformation()
      {
         var result = new CurrentUserInformation
         {
            DefaultSeason = new EntityInfoDto
            {
               Id = 6,
               Name = "2019-2020"
            },
            DefaultTeam = new EntityInfoDto
            {
               Id = 27,
               Name = "Kazio"
            }
         };

         return this.Ok(result);
      }
   }
}
