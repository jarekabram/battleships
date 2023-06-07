using Microsoft.AspNetCore.Components;

namespace BattleShips.Components;

public class BoardCell : ComponentBase
{
  [Parameter] public RenderFragment ChildContent { get; set; }
}