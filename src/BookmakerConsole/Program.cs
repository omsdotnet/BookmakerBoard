using System;

namespace BookmakerConsole
{
  class Program
  {
    static void Main(string[] args)
    {
      Console.WriteLine("Base URL:");
      var baseUrl = Console.ReadLine();
      Console.WriteLine("Login:");
      var login = Console.ReadLine();
      Console.WriteLine("Password:");
      var password = Console.ReadLine();

      var client = new BookMakerClient(baseUrl);

      var handler = new CommandHandler(client, login, password);
      handler.Initialize();

      var isExit = false;

      while(!isExit)
      {
        Console.WriteLine("Command:");
        var userText = Console.ReadLine();

        isExit = userText == "exit";

        if (!isExit)
        {
          try
          {
            var rezult = handler.Process(userText);

            Console.WriteLine(rezult);
          }
          catch(Exception ex)
          {
            Console.WriteLine(ex);
          }
        }
      }
    }
  }
}
