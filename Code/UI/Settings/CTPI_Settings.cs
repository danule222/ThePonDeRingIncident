using Godot;
using System;

public partial class CTPI_Settings : ITPI_Menu
{
	private CTPI_Button BTN_Back;
	private HSlider SLI_MasterVolume;
	private Label TXT_MasterValue;
	private HSlider SLI_MusicVolume;
	private Label TXT_MusicValue;
	private HSlider SLI_EffectsVolume;
	private Label TXT_EffectsValue;
	private HSlider SLI_TVVolume;
	private Label TXT_TVValue;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		BTN_Back = GetNode<CTPI_Button>("HBoxContainer/BTN_Back");
		VBoxContainer vbx = GetNode<VBoxContainer>("ScrollContainer/MarginContainer/VBoxContainer");
		SLI_MasterVolume = vbx.GetNode<HSlider>("MasterVolume/HSlider");
		TXT_MasterValue = vbx.GetNode<Label>("MasterVolume/Value");
		SLI_MusicVolume = vbx.GetNode<HSlider>("MusicVolume/HSlider");
		TXT_MusicValue = vbx.GetNode<Label>("MusicVolume/Value");
		SLI_EffectsVolume = vbx.GetNode<HSlider>("EffectsVolume/HSlider");
		TXT_EffectsValue = vbx.GetNode<Label>("EffectsVolume/Value");
		SLI_TVVolume = vbx.GetNode<HSlider>("TVVolume/HSlider");
		TXT_TVValue = vbx.GetNode<Label>("TVVolume/Value");

		SLI_MasterVolume.ValueChanged += MasterVolumeSlider;
		SLI_MusicVolume.ValueChanged += MusicVolumeSlider;
		SLI_EffectsVolume.ValueChanged += EffectsVolumeSlider;
		SLI_TVVolume.ValueChanged += TVVolumeSlider;
		BTN_Back.Pressed += BackPressed;

		SetValues();
		VisibilityChanged += SetValues;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private void SetValues()
	{
		SLI_MasterVolume.Value = Mathf.DbToLinear(NTPI_AL_AudioController.Instance.GetMasterVolume());
		TXT_MasterValue.Text = Mathf.FloorToInt(SLI_MasterVolume.Value * 10).ToString();
		SLI_MusicVolume.Value = Mathf.DbToLinear(NTPI_AL_AudioController.Instance.GetMusicVolume());
		TXT_MusicValue.Text = Mathf.FloorToInt(SLI_MusicVolume.Value * 10).ToString();
		SLI_EffectsVolume.Value = Mathf.DbToLinear(NTPI_AL_AudioController.Instance.GetEffectsVolume());
		TXT_EffectsValue.Text = Mathf.FloorToInt(SLI_EffectsVolume.Value * 10).ToString();
		SLI_TVVolume.Value = Mathf.DbToLinear(NTPI_AL_AudioController.Instance.GetTVVolume());
		TXT_TVValue.Text = Mathf.FloorToInt(SLI_TVVolume.Value * 10).ToString();
	}

	private void MasterVolumeSlider(double value)
	{
		NTPI_AL_AudioController.Instance.SetMasterVolume(Mathf.LinearToDb((float)value));
		TXT_MasterValue.Text = Mathf.FloorToInt(value * 10).ToString();
	}

	private void MusicVolumeSlider(double value)
	{
		NTPI_AL_AudioController.Instance.SetMusicVolume(Mathf.LinearToDb((float)value));
		TXT_MusicValue.Text = Mathf.FloorToInt(value * 10).ToString();
	}

	private void EffectsVolumeSlider(double value)
	{
		NTPI_AL_AudioController.Instance.SetEffectsVolume(Mathf.LinearToDb((float)value));
		TXT_EffectsValue.Text = Mathf.FloorToInt(value * 10).ToString();
	}

	private void TVVolumeSlider(double value)
	{
		NTPI_AL_AudioController.Instance.SetTVVolume(Mathf.LinearToDb((float)value));
		TXT_TVValue.Text = Mathf.FloorToInt(value * 10).ToString();
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
