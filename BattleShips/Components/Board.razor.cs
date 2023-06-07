using BattleShips.Model;
using Microsoft.AspNetCore.Components;

namespace BattleShips.Components;

public partial class Board : ComponentBase
{
  private readonly List<string> _letters = Enumerable.Range(65, 10)
    .Select(x => Convert.ToChar(x).ToString())
    .ToList();

  private readonly List<int> _numbers = Enumerable.Range(1, 10).ToList();
  private string[,] _board = new string[10, 10];
  private int _createdShips;
  private int _destroyedShips;
  private Random _random = new();
  private Dictionary<int, Ship> _ships = new();


  protected override void OnInitialized()
  {
    InitializeBoard();

    // Place ships
    PlaceShip(5); // Place Battleship
    PlaceShip(4); // Place Destroyer 1
    PlaceShip(4); // Place Destroyer 2
  }

  private void InitializeBoard()
  {
    // Initialize the board to be empty
    for (var xIndex = 0; xIndex < 10; xIndex++)
    for (var yIndex = 0; yIndex < 10; yIndex++)
      _board[xIndex, yIndex] = "🌊";
  }

  private void PlaceShip(int shipSize)
  {
    var isHorizontal = _random.Next(2) == 0;
    var ship = new Ship
    {
      Id = _createdShips,
      Size = shipSize
    };

    while (true)
    {
      int x, y;
      if (isHorizontal)
      {
        x = _random.Next(10 - shipSize + 1);
        y = _random.Next(10);
      }
      else
      {
        x = _random.Next(10);
        y = _random.Next(10 - shipSize + 1);
      }

      var allCellsEmpty = true;
      for (var i = 0; i < shipSize; i++)
      {
        if (isHorizontal)
        {
          if (_board[x + i, y] == "🌊")
          {
            continue;
          }

          allCellsEmpty = false;
          break;
        }

        if (_board[x, y + i] == "🌊")
        {
          continue;
        }

        allCellsEmpty = false;
        break;
      }

      if (!allCellsEmpty)
      {
        continue;
      }

      for (var i = 0; i < shipSize; i++)
        if (isHorizontal)
        {
          _board[x + i, y] = "💥";
          ship.Coordinates.Add(new Coordinate(x + i, y));
        }
        else
        {
          _board[x, y + i] = "💥";
          ship.Coordinates.Add(new Coordinate(x, y + i));
        }

      break;
    }

    _createdShips++;

    _ships.Add(ship.Id, ship);
  }

  private void HitShip(int xIndex, int yIndex)
  {
    if (_board[xIndex, yIndex] == "💥")
    {
      _board[xIndex, yIndex] = "🔥";
      var shipId = GetShipId(xIndex, yIndex);

      _ships[shipId].Hits++;

      if (_ships[shipId].IsDestroyed)
      {
        _destroyedShips++;
      }
    }

    StateHasChanged();
  }

  private int GetShipId(int x, int y)
  {
    return _ships.Values
      .FirstOrDefault(ship => ship.Coordinates
        .Any(c => c.X == x && c.Y == y))?.Id ?? 0;
  }
}