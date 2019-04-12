using System;
using BookmakerBoard.Infrastructure.Identity;
using BookmakerBoard.Logics;
using BookmakerBoard.Logics.Impl;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;

namespace BookmakerBoard
{
  /// <summary>
  /// 
  /// </summary>
  public class Startup
  {
    /// <summary>
    /// 
    /// </summary>
    /// <param name="configuration"></param>
    public Startup(IConfiguration configuration)
    {
      Configuration = configuration;
    }

    /// <summary>
    /// 
    /// </summary>
    public IConfiguration Configuration { get; }

    /// This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {

      services.AddIdentityCore<IdentityUser>()
        .AddSignInManager<SignInManager<IdentityUser>>()
        .AddUserManager<AspNetUserManager<IdentityUser>>()
        .AddUserStore<InMemoryUserStore>()
        .AddDefaultTokenProviders();
      
      // Identity Services
      services.AddSingleton<IUserStore<IdentityUser>, InMemoryUserStore>();
      services.AddSingleton<IPasswordHasher<IdentityUser>, PasswordHasher>();


      services.AddSingleton<IGame>(new Game());
      services.AddSingleton<IGameStorage, GameStorage>();

      var sp = services.BuildServiceProvider();
      var storage = sp.GetService<IGameStorage>();
      storage.Load();
      var game = sp.GetService<IGame>();
      game.CalculateBiddersCurrentScore();

      services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
      .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme,
        options =>
        {
          options.AccessDeniedPath = "/AccessDenied";
          options.Cookie.Name = "BookmakerBoardCookie";
          options.Cookie.HttpOnly = true;
          options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
          options.LoginPath = "/Login";
          options.ReturnUrlParameter = CookieAuthenticationDefaults.ReturnUrlParameter;
          options.SlidingExpiration = true;
        });

      services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

      // In production, the React files will be served from this directory
      services.AddSpaStaticFiles(configuration =>
      {
        configuration.RootPath = "ClientApp/build";
      });

      services.AddSwaggerGen(c =>
      {
        c.SwaggerDoc("v1", new Info { Title = "My API", Version = "v1" });
      });
    }

    /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IHostingEnvironment env)
    {
      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
      }
      else
      {
        app.UseExceptionHandler("/Error");
      }

      app.UseAuthentication();

      // Enable middleware to serve generated Swagger as a JSON endpoint.
      app.UseSwagger();

      // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), 
      // specifying the Swagger JSON endpoint.
      app.UseSwaggerUI(c =>
      {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
      });


      app.UseStaticFiles();
      app.UseSpaStaticFiles();

      app.UseMvc(routes =>
      {
        routes.MapRoute(
                  name: "default",
                  template: "{controller}/{action=Index}/{id?}");
      });

      app.UseSpa(spa =>
      {
        spa.Options.SourcePath = "ClientApp";

        if (env.IsDevelopment())
        {
          spa.UseReactDevelopmentServer(npmScript: "start");
        }
      });
    }
  }
}
