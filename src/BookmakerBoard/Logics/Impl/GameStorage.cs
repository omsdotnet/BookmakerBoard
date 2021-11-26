using Microsoft.AspNetCore.Hosting;
using System.IO;
using System.Runtime.Serialization.Json;

namespace BookmakerBoard.Logics.Impl
{
  public class GameStorage : IGameStorage
  {
    private readonly IGame gameEngine;
    private string dataFilePath;

    public GameStorage(IGame game, IWebHostEnvironment appEnvironment)
    {
      gameEngine = game;

      dataFilePath = appEnvironment.ContentRootPath + "/DataFiles/gameData.json";
    }

    public void Load()
    {
      if (File.Exists(dataFilePath))
      {
        DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(Game));

        FileStream fileStream = new FileStream(dataFilePath, FileMode.Open);

        var gameTmp = ser.ReadObject(fileStream) as Game;

        gameEngine.Bidders = gameTmp.Bidders;
        gameEngine.Teams = gameTmp.Teams;
        gameEngine.Rides = gameTmp.Rides;

        fileStream.Close();
      }
    }

    public void Save()
    {
      MemoryStream streamData = new MemoryStream();
      DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(Game));
      ser.WriteObject(streamData, gameEngine);

      streamData.Position = 0;
      StreamReader sr = new StreamReader(streamData);

      File.WriteAllText(dataFilePath, sr.ReadToEnd());
      streamData.Close();
    }
  }
}
