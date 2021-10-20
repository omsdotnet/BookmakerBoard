using BookmakerConsole.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace BookmakerConsole
{
  internal class BookMakerClient : IBookMakerClient
  {
    private readonly string baseAddress;
    private static HttpClient client = new HttpClient();

    public BookMakerClient(string baseaddress)
    {
      baseAddress = baseaddress;

      if (baseAddress.Last() != '/') baseAddress.Append('/');
    }

    public List<Bidder> GetAllBidders()
    {
      var result = Task.Run(() => client.GetAsync(baseAddress + "api/bidders/GetAll")).Result;

      if (result.IsSuccessStatusCode)
      {
        var biddersStr = Task.Run(() => result.Content.ReadAsStringAsync()).Result;

        return JsonConvert.DeserializeObject<List<Bidder>>(biddersStr);
      }

      return null;
    }

    public List<Team> GetAllTeams()
    {
      var result = Task.Run(() => client.GetAsync(baseAddress + "api/teams/GetAll")).Result;

      if (result.IsSuccessStatusCode)
      {
        var biddersStr = Task.Run(() => result.Content.ReadAsStringAsync()).Result;

        return JsonConvert.DeserializeObject<List<Team>>(biddersStr);
      }

      return null;
    }

    public List<Ride> GetAllRides()
    {
      var result = Task.Run(() => client.GetAsync(baseAddress + "api/rides/GetAll")).Result;

      if (result.IsSuccessStatusCode)
      {
        var biddersStr = Task.Run(() => result.Content.ReadAsStringAsync()).Result;

        return JsonConvert.DeserializeObject<List<Ride>>(biddersStr);
      }

      return null;
    }

    public Ride GetRide(uint id)
    {
      var result = Task.Run(() => client.GetAsync(baseAddress + "api/rides/GetById/" + id)).Result;

      if (result.IsSuccessStatusCode)
      {
        var biddersStr = Task.Run(() => result.Content.ReadAsStringAsync()).Result;

        return JsonConvert.DeserializeObject<Ride>(biddersStr);
      }

      return null;
    }

    public void UpdateRide(Ride item)
    {
      string jsonString = JsonConvert.SerializeObject(item);

      using (HttpContent httpContent = new StringContent(jsonString, Encoding.UTF8, "application/json"))
      {
        var result = Task.Run(() => client.PutAsync(baseAddress + "api/rides/" + item.Id, httpContent)).Result;

        result.EnsureSuccessStatusCode();
      }
    }


    public void Authentificate(string login, string password)
    {
      var addres = $"{baseAddress}api/Authentication/Login?login={HttpUtility.UrlEncode(login)}&password={HttpUtility.UrlEncode(password)}";

      var result = Task.Run(() => client.PostAsync(addres, null)).Result;

      result.EnsureSuccessStatusCode();
    }

  }
}
