using System;
using System.IO;
using System.Net.Http;
using BetDotNext.Activity;
using BetDotNext.Activity.Bet;
using BetDotNext.BotPlatform;
using BetDotNext.BotPlatform.Impl;
using BetDotNext.ExternalServices;
using BetDotNext.Services;
using BetDotNext.Setup;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MongoDB.Driver;
using Serilog;
using Serilog.Events;
using Telegram.Bot;

namespace BetDotNext
{
  public static class Program
  {
    private static IConfiguration _configuration;

    public static int Main(string[] args)
    {
      try
      {
        CreateHostBuilder(args).Build().Run();
      }
      catch (Exception ex)
      {
        Log.Fatal(ex, "Host terminated unexpectedly");
        return 1;
      }
      finally
      {
        Log.CloseAndFlush();
      }

      return 0;
    }

    private static IHostBuilder CreateHostBuilder(string[] args) =>
      Host.CreateDefaultBuilder(args)
        .ConfigureAppConfiguration((hostingContext, builder) =>
        {
          builder.SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
            .AddEnvironmentVariables();

          _configuration = builder.Build();

          var seqHost = _configuration["SEQ_HOST"];
          Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
            .Enrich.FromLogContext()
            .WriteTo.Console()
            .WriteTo.Seq(seqHost, bufferBaseFilename: "./logs/bot.log", batchPostingLimit: 100)
            .CreateLogger();

        }).ConfigureWebHostDefaults(webBuilder =>
        {
          webBuilder.ConfigureServices(services =>
          {
            var connection = _configuration["Mongo"];
            var database = _configuration["DB"];
            var telegramToken = _configuration["TelegramToken"];

            var handler = new HttpClientHandler
            {
              ServerCertificateCustomValidationCallback = (message, certificate2, arg3, arg4) => true
            };

            services.AddSingleton<ITelegramBotClient>(new TelegramBotClient(telegramToken, new HttpClient(handler)));
            services.AddSingleton(_ => new MongoClient(connection).GetDatabase(database).MongoDbInit());

            services.AddSingleton<BetService>();
            services.AddSingleton<QueueMessagesService>();
            services.AddSingleton<ConversationService>();

            services.AddHostedService<BetToTelegramService>();

            services.AddHttpClient<BetPlatformService>(client =>
            {
              client.BaseAddress = new Uri(_configuration["BetPlatform"]);
            });

            services.AddSingleton<IBotStorage, BotStorageInMemory>();
            services.AddSingleton<IBot, Bot>();
            services.AddSingleton<IBotMediator, BotMediator>();

            services.AddSingleton<BetActivity>();
            services.AddSingleton<StartActivity>();
            services.AddSingleton<RemoveBetActivity>();
            services.AddSingleton<CreatedBetActivity>();
            services.AddSingleton<ConfirmRemoveBetActivity>();
            services.AddSingleton<HelpActivity>();
            services.AddSingleton<RemoveAllActivity>();
            services.AddSingleton<ScoreActivity>();

          }).Configure(app =>
          {
            app.ApplicationServices.GetRequiredService<IBot>()
              .AddActivity("/start", typeof(StartActivity))
              .AddActivity("/bet", typeof(BetActivity))
              .AddActivity("/removebet", typeof(RemoveBetActivity))
              .AddActivity("/help", typeof(HelpActivity))
              .AddActivity("/score", typeof(ScoreActivity))
              .AddActivity("/removeall", typeof(RemoveAllActivity));

            app.ApplicationServices.GetRequiredService<BetService>().Start();
          }).UseSerilog();
        });
  }
}