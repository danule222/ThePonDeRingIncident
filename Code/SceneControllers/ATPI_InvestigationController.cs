using Godot;

public partial class ATPI_InvestigationController : Node3D
{
  public CTPI_HUD HUD;

  public override void _Ready()
  {
    HUD = GetNode<CTPI_HUD>("UI_HUD");
  }
}
