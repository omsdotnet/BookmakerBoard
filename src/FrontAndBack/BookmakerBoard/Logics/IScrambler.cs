namespace BookmakerBoard.Logics
{
  public interface IScrambler
  {
    string GetHash(string name, string pass);
  }
}