namespace Beryllium.WebApi.Controllers
{
   using System;
   using Beryllium.WebApi.Authorization;
   using Microsoft.AspNetCore.Authorization;
   using Microsoft.AspNetCore.Mvc;

   [Route("api/[controller]")]
   [ApiController]
   public class TestController : ControllerBase
   {
      [HttpGet]
      [Route("private")]
      [Authorize]
      public IActionResult Private()
      {
         return Json(new
         {
            Message = "Hello from a private endpoint! You need to be authenticated to see this."
         });
      }

      private IActionResult Json(object p)
      {
         throw new NotImplementedException();
      }

      [HttpGet]
      [Route("private-scoped")]
      [Authorize(RankixApiScopes.ViewTrainings)]
      public IActionResult Scoped()
      {
         return Json(new
         {
            Message = "Hello from a private endpoint! You need to be authenticated and have a scope of view:trainings to see this."
         });
      }
   }
}