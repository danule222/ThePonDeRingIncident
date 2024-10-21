using Godot;
using GodotInk;
using System;

[GlobalClass]
public partial class RTPI_Interrogation : Resource
{
  [Export]
  public ECharacter Character;
  [Export]
  public InkStory Story;
  [Export]
  public int TensionLevelStart;
}
