using BookmakerBoard.Logics.Impl;
using BookmakerBoard.Models;
using NUnit.Framework;
using System.Collections.Generic;

namespace Tests
{
  public class GameTest
  {
    private Game gameEngine;

    [OneTimeSetUp]
    public void Setup()
    {
      gameEngine = new Game();
      gameEngine.Teams = new List<Team>()
      {
        new Team() { Id = 0, Name = "Winner" },
        new Team() { Id = 1, Name = "Looser" }
      };
      gameEngine.Bidders = new List<Bidder>()
      {
        new Bidder() { Id = 0, Name = "Luky", StartScore = 1000 },
        new Bidder() { Id = 1, Name = "Not Luky", StartScore = 1000 }
      };
      gameEngine.Rides = new List<Ride>()
      {
        new Ride()
        {
          Id = 0,
          Number = 1,
          WinnerTeams = new List<uint> { 0 },
          Rates = new List<Rate>()
          {
            new Rate() { Id = 0, Bidder = gameEngine.Bidders[0], Team = gameEngine.Teams[0].Id, RateValue = 100 },
            new Rate() { Id = 1, Bidder = gameEngine.Bidders[1], Team = gameEngine.Teams[1].Id, RateValue = 100 },
          }
        }
      };
    }

    [Test]
    public void CalculateBiddersCurrentScoreTest()
    {
      gameEngine.CalculateBiddersCurrentScore();

      Assert.AreEqual(1200, gameEngine.Bidders[0].CurrentScore);
      Assert.AreEqual(900,  gameEngine.Bidders[1].CurrentScore);
    }
  }
}