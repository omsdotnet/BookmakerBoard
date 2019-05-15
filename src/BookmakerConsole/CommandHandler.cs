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
      if (userText == "update")
      {
        Initialize();
        return "OK - updated";
      }

      var textParts = userText
        .Split("-", StringSplitOptions.RemoveEmptyEntries)
        .ToList();

      if (textParts.Count == 4)
      {
        return UpdateRates(textParts);
      }
      else if (textParts.Count == 3)
      {
        return DeleteRatesForSpeaker(textParts);
      }
      else if (textParts.Count == 2)
      {
        return DeleteRatesForBidder(textParts);
      }
      else
      {
        return "ERROR - wrong format: [bidder] - [speaker] - [rate] - [ride]";
      }
    }

    private string DeleteRatesForBidder(List<string> textParts)
    {
      var userBidder = textParts[0].Trim();
      var userRate = Convert.ToUInt32(textParts[1].Trim());

      if (userRate != 0)
      {
        return $"ERROR - rate: {userRate} != 0";
      }

      var bidder = bidders.SingleOrDefault(x => x.Name.ToLower() == userBidder.ToLower());

      if (bidder == null)
      {
        return "ERROR - bidder not found";
      }

      var ridesToUpdate = rides
        .Where(x => x.Rates.Any(y => y.Bidder.Id == bidder.Id))
        .Select(x => x.Id)
        .ToList();


      foreach (var rideId in ridesToUpdate)
      {
        var ride = client.GetRide(rideId);

        var ratesToDelete = ride.Rates
          .Where(x => x.Bidder.Id == bidder.Id)
          .ToList();

        foreach (var rateToDelete in ratesToDelete)
        {
          ride.Rates.Remove(rateToDelete);
        }

        client.UpdateRide(ride);
      }


      bidders = client.GetAllBidders();
      rides = client.GetAllRides();

      var currentScore = bidders.Single(x => x.Id == bidder.Id).CurrentScore;

      return $"OK - current score: {currentScore}";
    }

    private string DeleteRatesForSpeaker(List<string> textParts)
    {
      var userBidder = textParts[0].Trim();
      var userSpeaker = textParts[1].Trim();
      var userRate = Convert.ToUInt32(textParts[2].Trim());

      if (userRate != 0)
      {
        return $"ERROR - rate: {userRate} != 0";
      }

      var bidder = bidders.SingleOrDefault(x => x.Name.ToLower() == userBidder.ToLower());

      if (bidder == null)
      {
        return "ERROR - bidder not found";
      }

      var speaker = speakers.SingleOrDefault(x => x.Name.ToLower().Replace('-', ' ') == userSpeaker.ToLower());

      if (speaker == null)
      {
        return "ERROR - speaker not found";
      }


      var ridesToUpdate = rides
        .Where(x => x.Rates.Any(y => y.Bidder.Id == bidder.Id && y.Team == speaker.Id))
        .Select(x => x.Id)
        .ToList();


      foreach(var rideId in ridesToUpdate)
      {
        var ride = client.GetRide(rideId);

        var ratesToDelete = ride.Rates
          .Where(x => x.Bidder.Id == bidder.Id && x.Team == speaker.Id)
          .ToList();

        foreach(var rateToDelete in ratesToDelete)
        {
          ride.Rates.Remove(rateToDelete);
        }

        client.UpdateRide(ride);
      }


      bidders = client.GetAllBidders();
      rides = client.GetAllRides();

      var currentScore = bidders.Single(x => x.Id == bidder.Id).CurrentScore;

      return $"OK - current score: {currentScore}";
    }

    private string UpdateRates(List<string> textParts)
    {
      var userBidder = textParts[0].Trim();
      var userSpeaker = textParts[1].Trim();
      var userRate = Convert.ToUInt32(textParts[2].Trim());
      var userRide = Convert.ToUInt32(textParts[3].Trim());


      var bidder = bidders.SingleOrDefault(x => x.Name.ToLower() == userBidder.ToLower());

      if (bidder == null)
      {
        return "ERROR - bidder not found";
      }

      var speaker = speakers.SingleOrDefault(x => x.Name.ToLower().Replace('-', ' ') == userSpeaker.ToLower());

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
        if (bidder.CurrentScore + rate.RateValue < userRate)
        {
          return $"ERROR - rate: {bidder.CurrentScore} < {userRate}";
        }

        rate.RateValue = userRate;
      }
      else
      {
        if (bidder.CurrentScore < userRate)
        {
          return $"ERROR - rate: {bidder.CurrentScore} < {userRate}";
        }

        var maxId = ride.Rates.Any() ? ride.Rates.Max(x => x.Id) : 0;

        var rateItem = new Rate()
        {
          Id = maxId++,
          Bidder = bidder,
          RateValue = userRate,
          Team = speaker.Id
        };

        ride.Rates.Add(rateItem);
      }


      client.UpdateRide(ride);

      bidders = client.GetAllBidders();
      rides = client.GetAllRides();

      var currentScore = bidders.Single(x => x.Id == bidder.Id).CurrentScore;

      return $"OK - current score: {currentScore}";
    }
  }
}
