using Godot;
using System;

public partial class CTPI_HUD : Control
{
	[Export]
	private Texture2D ReticleDefault;
	[Export]
	private Texture2D ReticleEvidence;

	private TextureRect IMG_Reticle;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		IMG_Reticle = GetNode<TextureRect>("IMG_Reticle");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void SetReticle(bool isEvidence)
	{
		if (isEvidence)
			IMG_Reticle.Texture = ReticleEvidence;
		else
			IMG_Reticle.Texture = ReticleDefault;
	}
}
