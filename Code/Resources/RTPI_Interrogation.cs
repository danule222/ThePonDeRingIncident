using Godot;
using Godot.Collections;
using GodotInk;

[GlobalClass]
public partial class RTPI_Interrogation : Resource
{
  [Export]
  public RTPI_Character Character;
  [Export]
  public Array<InkStory> Stories;
  [Export]
  public int TensionLevelStart;
}
