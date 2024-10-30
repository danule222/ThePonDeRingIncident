using Godot;

public partial class ATPI_InvestigationController : Node3D
{
  public CTPI_HUD HUD;

  private Timer Countdown;

  public override void _Ready()
  {
    HUD = GetNode<CTPI_HUD>("UI_HUD");
    Countdown = GetNode<Timer>("Countdown");

    Countdown.Timeout += CountdownEnd;
    Countdown.Start();
  }

  public override void _Process(double delta)
  {
    HUD.SetTime((float)Countdown.TimeLeft);
  }

  private void CountdownEnd()
  {
    // TODO: Implement end
  }
}
