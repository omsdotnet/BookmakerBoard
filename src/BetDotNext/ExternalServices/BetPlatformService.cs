using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using BetDotNext.Activity;
using BetDotNext.ExternalServices.Dto;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace BetDotNext.ExternalServices
{
  public class BetPlatformService
  {
    private const string TeamUrl = "api/teams/GetAll";
    private const string BiddersUrl = "api/bidders/GetAll";
    private const string RidesUrl = "api/rides/GetAll";
    private const string RideByIdUrl = "api/rides/GetById/";

    private readonly HttpClient _httpClient;
    private readonly ILogger<BetPlatformService> _logger;

    private readonly string _betPass;
    private readonly string _betLogin;

    public BetPlatformService(HttpClient httpClient, ILogger<BetPlatformService> logger, IConfiguration configuration)
    {
      _httpClient = httpClient;
      _logger = logger;

      _betLogin = configuration["bet_login"];
      _betPass = configuration["bet_pass"];
    }

    private async Task AuthenticationAsync()
    {
      var url = $"api/Authentication/Login?login={HttpUtility.UrlEncode(_betLogin)}&password={HttpUtility.UrlEncode(_betPass)}";
      var result = await _httpClient.PostAsync(url, null);
      var _ = await result.Content.ReadAsStringAsync();
      result.EnsureSuccessStatusCode();
    }

    private async Task<IList<Bidder>> BiddersAsync()
    {
      var result = await _httpClient.GetAsync(BiddersUrl);
      var s = await result.Content.ReadAsStringAsync();
      return JsonConvert.DeserializeObject<IList<Bidder>>(s);
    }

    private async Task<IList<Team>> TeamsAsync()
    {
      var result = await _httpClient.GetAsync(TeamUrl);
      var s = await result.Content.ReadAsStringAsync();
      return JsonConvert.DeserializeObject<IList<Team>>(s);
    }

    private async Task<IList<Ride>> RidesAsync()
    {
      var result = await _httpClient.GetAsync(RidesUrl);
      var s = await result.Content.ReadAsStringAsync();
      return JsonConvert.DeserializeObject<IList<Ride>>(s);
    }

    private async Task<Ride> GetRide(uint id)
    {
      var result = await _httpClient.GetAsync(RideByIdUrl + id);
      var biddersStr = await result.Content.ReadAsStringAsync();
      return JsonConvert.DeserializeObject<Ride>(biddersStr);
    }

    private async Task UpdateRide(Ride item)
    {
      string jsonString = JsonConvert.SerializeObject(item);
      using HttpContent httpContent = new StringContent(jsonString, Encoding.UTF8, "application/json");
      var result = await _httpClient.PutAsync("api/rides/" + item.Id, httpContent);
      result.EnsureSuccessStatusCode();
    }

    private async Task AddBidder(Bidder bidder)
    {
      string jsonString = JsonConvert.SerializeObject(bidder);
      using HttpContent httpContent = new StringContent(jsonString, Encoding.UTF8, "application/json");
      var result = await _httpClient.PostAsync("api/Bidders", httpContent);
      result.EnsureSuccessStatusCode();
    }

    public async Task<long?> CreateBetAsync(CreateBet bet)
    {
      await AuthenticationAsync();

      var bidders = await BiddersAsync();
      var bidder = bidders.SingleOrDefault(x => x.Name.ToLower() == bet.Bidder.ToLower());
      if (bidder == null)
      {
        _logger.LogDebug("Created new bidder {0}", bet.Bidder);

        var bidderId = !bidders.Any() ? 0 : bidders.Max(p => p.Id);
        bidder = new Bidder { Id = ++bidderId, Name = bet.Bidder, CurrentScore = 1000, StartScore = 1000 };
        await AddBidder(bidder);

        bidders = await BiddersAsync();
      }

      var teams = await TeamsAsync();

      var speaker = teams.SingleOrDefault(x => x.Name.Replace('-', ' ').Equals(bet.Speaker, StringComparison.InvariantCultureIgnoreCase));
      if (speaker == null)
      {
        _logger.LogError("Not found speaker");
        throw new UnexpectedFormatMessageException(StringsResource.SpeakerNotFound);
      }

      var rides = await RidesAsync();
      var rideId = rides.SingleOrDefault(x => x.Number == bet.Ride)?.Id;
      if (!rideId.HasValue)
      {
        _logger.LogError("Not found ride");
        throw new UnexpectedFormatMessageException(StringsResource.IncorectNomination);
      }

      var ride = await GetRide(rideId.Value);
      var rate = ride.Rates.SingleOrDefault(x => x.Bidder.Id == bidder?.Id && x.Team == speaker.Id);
      if (bet.Rate == 0)
      {
        throw new UnexpectedFormatMessageException(StringsResource.BetRateIsNotZerro);
      }
      else if (rate != null)
      {
        if (bidder.CurrentScore + rate.RateValue < bet.Rate)
        {
          _logger.LogError($"rate: {bidder.CurrentScore} < {bet.Rate}");
          throw new UnexpectedFormatMessageException(StringsResource.BetRateIsNotEnough);
        }

        rate.RateValue = bet.Rate;
      }
      else
      {
        if (bidder.CurrentScore < bet.Rate)
        {
          _logger.LogError($"rate: {bidder.CurrentScore} < {bet.Rate}");
          throw new UnexpectedFormatMessageException(StringsResource.BetRateIsNotEnough);
        }

        var maxId = ride.Rates.Any() ? ride.Rates.Max(x => x.Id) : 0;
        var rateItem = new Rate
        {
          Id = ++maxId,
          Bidder = bidder,
          RateValue = bet.Rate,
          Team = speaker.Id
        };

        ride.Rates.Add(rateItem);
      }

      await UpdateRide(ride);

      bidders = await BiddersAsync();

      return bidders.Single(x => x.Id == bidder?.Id).CurrentScore;
    }

    public async Task<long?> DeleteRateForBetAsync(CreateBet bet)
    {
      if (bet.Rate != 0)
      {
        _logger.LogError($"ERROR - rate: {bet.Rate} != 0");
        throw new UnexpectedFormatMessageException(StringsResource.BetRateMustZero);
      }

      await AuthenticationAsync();

      var bidders = await BiddersAsync();
      var bidder = bidders.SingleOrDefault(x => x.Name.ToLower() == bet.Bidder.ToLower());
      if (bidder == null)
      {
        _logger.LogError("ERROR - bidder not found");
        throw new UnexpectedFormatMessageException(StringsResource.NotExistBidder);
      }

      var teams = await TeamsAsync();
      var speaker = teams.SingleOrDefault(x => x.Name.Replace("-", " ").ToLower() == bet.Speaker.ToLower());
      if (speaker == null)
      {
        _logger.LogError("ERROR - speaker not found");
        throw new UnexpectedFormatMessageException(StringsResource.SpeakerNotFound);
      }

      var rides = await RidesAsync();
      var rideId = rides.FirstOrDefault(x => x.Number == bet.Ride)?.Id;
      var ridesToUpdate = rides
        .Where(x => x.Rates.Any(y => y.Bidder.Id == bidder.Id && y.Team == speaker.Id &&
                                     (rideId.HasValue && x.Id == rideId.Value || !rideId.HasValue)))
        .Select(x => x.Id)
        .ToList();

      foreach (var id in ridesToUpdate)
      {
        var ride = await GetRide(id);
        var ratesToDelete = ride.Rates
          .Where(x => x.Bidder.Id == bidder.Id && x.Team == speaker.Id)
          .ToList();

        foreach (var rateToDelete in ratesToDelete)
        {
          ride.Rates.Remove(rateToDelete);
        }

        await UpdateRide(ride);
      }

      bidders = await BiddersAsync();
      var currentScore = bidders.Single(x => x.Id == bidder.Id)?.CurrentScore;

      _logger.LogInformation("Current score a participant {0} = {1} from", bet.Bidder, currentScore);

      return currentScore;
    }

    public async Task<string> DeleteAllBetAsync(string bidderName)
    {
      await AuthenticationAsync();

      var bidders = await BiddersAsync();
      var bidder = bidders.SingleOrDefault(x => x.Name.ToLower() == bidderName.ToLower());
      if (bidder == null)
      {
        _logger.LogError("ERROR - bidder not found");
        return StringsResource.NotExistBidder;
      }

      var rides = await RidesAsync();
      var ridesToUpdate = rides
        .Where(x => x.Rates.Any(y => y.Bidder.Id == bidder.Id))
        .Select(x => x.Id)
        .ToList();

      foreach (var id in ridesToUpdate)
      {
        var ride = await GetRide(id);
        var ratesToDelete = ride.Rates
          .Where(x => x.Bidder.Id == bidder.Id)
          .ToList();

        foreach (var rateToDelete in ratesToDelete)
        {
          ride.Rates.Remove(rateToDelete);
        }

        await UpdateRide(ride);
      }

      bidders = await BiddersAsync();
      var currentScore = bidders.Single(x => x.Id == bidder.Id).CurrentScore;

      _logger.LogInformation("Current score a participant {0} = {1} from", bidderName, currentScore);

      return string.Format(StringsResource.CurrentScoreRemoveMessage, currentScore);
    }

    public async Task<string> CurrentScoreAsync(string currentBidder)
    {
      await AuthenticationAsync();

      var bidders = await BiddersAsync();
      var bidder = bidders.SingleOrDefault(x => x.Name.ToLower() == currentBidder.ToLower());
      if (bidder == null)
      {
        _logger.LogError("ERROR - bidder not found");
        return StringsResource.NotExistBidder;
      }

      return string.Format(StringsResource.CurrentScoreMessage, bidder.CurrentScore);
    }
  }
}
