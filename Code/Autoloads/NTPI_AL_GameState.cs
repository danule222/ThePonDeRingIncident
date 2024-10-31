using Godot;
using System.Collections.Generic;

public partial class NTPI_AL_GameState : Node
{
  public static NTPI_AL_GameState Instance { get; private set; }

  public List<RTPI_Evidence> Evidences { get; private set; }

  public override void _Ready()
  {
    Evidences = new List<RTPI_Evidence>();

    Instance = this;
  }

  public void AddEvidence(RTPI_Evidence evidence)
  {
    Evidences.Add(evidence);
  }
}