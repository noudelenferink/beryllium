namespace Beryllium.WebApi.Controllers
{
   using Microsoft.AspNetCore.Mvc;
   using Microsoft.Extensions.Logging;
   using Microsoft.Extensions.DependencyInjection;
   using System;

   [ApiController]
   public abstract class BaseApiController : Controller
   {
      //private ILogger<T> logger;
      //public ILogger<T> Logger => logger ?? (logger = HttpContext?.RequestServices.GetService<ILogger<GetType()>>()); 

      private ILogger logger;
      public ILogger Logger => logger ?? (logger = this.GetLogger());
      private ILogger GetLogger()
      {
         //  return typeof(ILogger).MakeGenericType(GetType());
         return new LoggerFactory().CreateLogger(GetType());
      }
   }
}
