using Godot;
using Godot.Collections;

[GlobalClass]
public partial class RTPI_Character : Resource
{
  [Export]
  public ECharacter Name;
  [Export]
  public Texture2D Portrait;
  [Export]
  public Dictionary<EEmotion, Texture2D> Emotions;
}
