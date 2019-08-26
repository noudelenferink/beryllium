namespace Beryllium.WebApi
{
   using System.Linq;
   using System.Threading.Tasks;
   using Beryllium.Core;
   using Beryllium.EntityFrameworkCore;
   using Beryllium.WebApi.Authorization;
   using Microsoft.AspNetCore.Authentication.JwtBearer;
   using Microsoft.AspNetCore.Authorization;
   using Microsoft.AspNetCore.Builder;
   using Microsoft.AspNetCore.Hosting;
   using Microsoft.AspNetCore.Mvc;
   using Microsoft.EntityFrameworkCore;
   using Microsoft.Extensions.Configuration;
   using Microsoft.Extensions.DependencyInjection;
   using Microsoft.Extensions.Logging;
   using Swashbuckle.AspNetCore.Swagger;

   public class Startup
   {
      public Startup(IHostingEnvironment env)
      {
         var builder = new ConfigurationBuilder()
             .SetBasePath(env.ContentRootPath)
             .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
             .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
             .AddEnvironmentVariables();

         Configuration = builder.Build();
      }

      public IConfiguration Configuration { get; }

      // This method gets called by the runtime. Use this method to add services to the container.
      public void ConfigureServices(IServiceCollection services)
      {
         services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

         string domain = $"https://{Configuration["Auth0:Domain"]}/";
         services.AddAuthentication(options =>
         {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

         }).AddJwtBearer(options =>
         {
            options.Authority = domain;
            options.Audience = Configuration["Auth0:ApiIdentifier"];
         });

         services.AddAuthorization(options =>
         {
            typeof(RankixApiScopes).GetFields().ToList().ForEach(x =>
            {
               var scopeName = x.GetValue(null).ToString();
               options.AddPolicy(scopeName, policy => policy.Requirements.Add(new HasScopeRequirement(scopeName, domain)));
            });

         });

         services.AddSwaggerGen(c =>
         {
            c.SwaggerDoc("v1", new Info { Title = "My API", Version = "v1" });
         });

         services.AddDbContext<RankixContext>(options => 
            options
               .UseLazyLoadingProxies()
               .UseMySQL(Configuration.GetConnectionString("Default"))
         );

         // register the scope authorization handler
         services.AddSingleton<IAuthorizationHandler, HasScopeHandler>();
         services.AddScoped(typeof(IRepository<>), typeof(BaseRepository<>));

      }

      // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
      public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
      {
         app.UseMvc();

         app.UseSwagger();
         app.UseSwaggerUI(c =>
         {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
         });

         app.Run(async (context) => await Task.Run(() => context.Response.Redirect("/swagger")));
      }
   }
}
