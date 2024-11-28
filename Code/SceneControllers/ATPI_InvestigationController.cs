using Godot;

public partial class ATPI_InvestigationController : Node3D
{
  [Export]
  private int TotalEvidences;

  public CTPI_HUD HUD { get; private set; }
  public NTPI_AL_GameState GameState { get; private set; }

  private Timer Countdown;
  private Timer MoveToInterrogation;
  private int EvidencesFound;
  private bool InvestigationEnded;

  public override void _Ready()
  {
    HUD = GetNode<CTPI_HUD>("UI_HUD");
    Countdown = GetNode<Timer>("Countdown");
    MoveToInterrogation = GetNode<Timer>("MoveToInterrogation");
    GameState = NTPI_AL_GameState.Instance;

    Countdown.Timeout += CountdownEnd;
    Countdown.Start();
    MoveToInterrogation.Timeout += InvestigationEnd;

    EvidencesFound = 0;
    HUD.AddEvidence(EvidencesFound, TotalEvidences);
    InvestigationEnded = false;

    // Music
    NTPI_AL_AudioController.Instance.PlayBackgroundMusic(EGameState.Investigation);
  }

  public override void _Process(double delta)
  {
    HUD.SetTime((float)Countdown.TimeLeft);
  }

  public void AddEvidence(RTPI_Evidence evidence)
  {
    HUD.SetEvidence(evidence);
    GameState.AddEvidence(evidence);

    EvidencesFound++;
    HUD.AddEvidence(EvidencesFound, TotalEvidences);

    if (EvidencesFound == TotalEvidences)
      EverythingFound();
  }

  private void CountdownEnd()
  {
    HUD.SetMessage("Time's up!");
    MoveToInterrogation.Start();
  }

  private void EverythingFound()
  {
    HUD.SetMessage("You found every evidence!");
    MoveToInterrogation.Start();
    Countdown.Paused = true;
    InvestigationEnded = true;
  }

  private void InvestigationEnd()
  {
    GetTree().ChangeSceneToFile("res://Scenes/S_Interrogation.tscn");
  }

  public void Pause()
  {
    HUD.ShowPauseMenu();
  }
}
