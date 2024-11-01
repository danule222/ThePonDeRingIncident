using Godot;
using System;

public partial class CTPI_Settings : ITPI_Menu
{
	private CTPI_Button BTN_Back;
	private HSlider SLI_MusicVolume;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		BTN_Back = GetNode<CTPI_Button>("HBoxContainer/BTN_Back");
		SLI_MusicVolume = GetNode<HSlider>("VBoxContainer/MusicVolume/HSlider");

		SLI_MusicVolume.ValueChanged += MusicVolumeSlider;
		BTN_Back.Pressed += BackPressed;

		SLI_MusicVolume.Value = NTPI_AL_MusicController.Instance.GetMusicVolume();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private void MusicVolumeSlider(double value)
	{
		NTPI_AL_MusicController.Instance.SetMusicVolume((float)value);
	}

	private void BackPressed()
	{
		if (ParentMenu != null)
		{
			Visible = false;
			ParentMenu.Visible = true;
		}
	}
}
