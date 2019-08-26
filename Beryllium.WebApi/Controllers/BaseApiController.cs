namespace Beryllium.WebApi.Controllers
{
   using Microsoft.AspNetCore.Mvc;
   using Microsoft.Extensions.Logging;
   using Microsoft.Extensions.DependencyInjection;

   [ApiController]
   public abstract class BaseApiController<T> : Controller where T : BaseApiController<T>
   {
      private ILogger<T> logger;

      public ILogger<T> Logger => logger ?? (logger = HttpContext?.RequestServices.GetService<ILogger<T>>()); 
   }
}
