using System.Collections.Generic;
using BookmakerConsole.Models;

namespace BookmakerConsole
{
  internal interface IBookMakerClient
  {
    void Authentificate(string login, string password);
    List<Bidder> GetAllBidders();
    List<Ride> GetAllRides();
    List<Team> GetAllTeams();
    Ride GetRide(uint id);
    void UpdateRide(Ride item);
  }
}