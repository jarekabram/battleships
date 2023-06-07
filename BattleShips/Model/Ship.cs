namespace BattleShips.Model;

public class Ship
{
  public int Id { get; set; }
  public int Size { get; set; }
  public List<Coordinate> Coordinates { get; set; } = new();
  public int Hits { get; set; } = 0;

  public bool IsDestroyed => Hits >= Size;
}