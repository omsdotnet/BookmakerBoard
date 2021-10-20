
namespace BookmakerConsole.Models
{
  public class Rate
  {
    public uint Id { get; set; }

    public Bidder Bidder { get; set; }

    public uint Team { get; set; }

    public uint RateValue { get; set; }
  }
}
