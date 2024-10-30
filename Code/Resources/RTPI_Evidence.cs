using Godot;

[GlobalClass]
public partial class RTPI_Evidence : Resource
{
  [Export]
  public string Name;
  [Export(PropertyHint.MultilineText)]
  public string Description;
  [Export]
  public Texture2D Photo;
}
