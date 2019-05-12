using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BookmakerConsole.Models;

namespace BookmakerConsole
{
  internal class CommandHandler
  {
    private IBookMakerClient client;
    private List<Bidder> bidders;
    private List<Team> speakers;
    private List<Ride> rides;

    public CommandHandler(IBookMakerClient client)
    {
      this.client = client;
    }

    internal void Initialize()
    {
      bidders = client.GetAllBidders();
      speakers = client.GetAllTeams();
      rides = client.GetAllRides();
    }


    internal string Process(string userText)
    {
      var textParts = userText
        .Split("-", StringSplitOptions.RemoveEmptyEntries)
        .ToList();

      if (textParts.Count != 4)
      {
        return "ERROR - wrong format: [bidder] - [speaker] - [rate] - [ride]";
      }

      var userBidder = textParts[0].Trim();
      var userSpeaker = textParts[1].Trim();
      var userRate = Convert.ToUInt32(textParts[2].Trim());
      var userRide = Convert.ToUInt32(textParts[3].Trim());


      var bidder = bidders.SingleOrDefault(x => x.Name.ToLower() == userBidder.ToLower());

      if (bidder == null)
      {
        return "ERROR - bidder not found";
      }

      if (bidder.CurrentScore < userRate)
      {
        return $"ERROR - rate: {bidder.CurrentScore} < {userRate}";
      }

      var speaker = speakers.SingleOrDefault(x => x.Name.ToLower() == userSpeaker.ToLower());

      if (speaker == null)
      {
        return "ERROR - speaker not found";
      }

      var rideId = rides.SingleOrDefault(x => x.Number == userRide)?.Id;

      if (rideId == null)
      {
        return "ERROR - ride not found";
      }

      var ride = client.GetRide(rideId.Value);


      var rate = ride.Rates.SingleOrDefault(x => x.Bidder.Id == bidder.Id && x.Team == speaker.Id);

      if (userRate == 0)
      {
        if (rate != null)
        {
          ride.Rates.Remove(rate);
        }
      }
      else if (rate != null)
      {
        rate.RateValue = userRate;
      }
      else
      {
        var maxId = ride.Rates.Max(x => x.Id);

        var rateItem = new Rate()
        {
          Id = maxId++,
          Bidder = bidder,
          RateValue = userRate,
          Team = speaker.Id
        };
      }


      client.UpdateRide(ride);

      bidders = client.GetAllBidders();

      var currentScore = bidders.Single(x => x.Id == bidder.Id).CurrentScore;

      return $"OK - current score: {currentScore}";
    }

  }
}
