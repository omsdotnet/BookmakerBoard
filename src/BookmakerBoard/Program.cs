using BookmakerBoard;
using BookmakerBoard.Infrastructure.Identity;
using BookmakerBoard.Logics;
using BookmakerBoard.Logics.Impl;

using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;

WebApplicationBuilder? builder = WebApplication.CreateBuilder(args);

builder.Services.AddMvc(options => options.EnableEndpointRouting = false);
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddIdentityCore<IdentityUser>()
    .AddSignInManager<SignInManager<IdentityUser>>()
    .AddUserManager<AspNetUserManager<IdentityUser>>()
    .AddUserStore<InMemoryUserStore>()
    .AddDefaultTokenProviders();

builder.Services.AddSingleton<IUserStore<IdentityUser>, InMemoryUserStore>()
    .AddSingleton<IPasswordHasher<IdentityUser>, PasswordHasher>()
    .AddSingleton<IGame>(new Game())
    .AddSingleton<IGameStorage, GameStorage>()
    .AddHostedService<StoreHostedService>();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
    {
        options.AccessDeniedPath = "/AccessDenied";
        options.Cookie.Name = "BookmakerBoardCookie";
        options.Cookie.HttpOnly = true;
        options.ExpireTimeSpan = TimeSpan.FromMinutes(600);
        options.LoginPath = "/Login";
        options.ReturnUrlParameter = CookieAuthenticationDefaults.ReturnUrlParameter;
        options.SlidingExpiration = true;
    });

builder.Services.AddSpaStaticFiles(configuration =>
{
    configuration.RootPath = "ClientApp/build";
});

if (builder.Environment.IsDevelopment())
{
    builder.Services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "BookmakerBoard API", Version = "v1" });
    });
}

WebApplication? app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage()
       .UseSwagger()
       .UseSwaggerUI(options => options.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1"));
}
else
{
    app.UseHsts();
    app.UseExceptionHandler("/Error");
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseSpaStaticFiles();
app.UseRouting();
app.UseAuthentication()
   .UseAuthorization();

app.UseMvc(routes =>
{
    routes.MapRoute(
              name: "default",
              template: "{controller}/{action=Index}/{id?}");
});

app.UseSpa(spa =>
  {
      spa.Options.SourcePath = "ClientApp";

      if (app.Environment.IsDevelopment())
      {
          spa.UseReactDevelopmentServer(npmScript: "start");
      }
  });


app.Run();