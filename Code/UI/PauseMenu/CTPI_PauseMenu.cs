using Godot;
using System;

public partial class CTPI_PauseMenu : ITPI_Menu
{
	[Export]
	public Input.MouseModeEnum MouseModeWhenResuming;

	private CTPI_Button BTN_Resume;
	private CTPI_Button BTN_Options;
	private CTPI_Button BTN_TitleScreen;
	private Panel PNL_PauseMenu;
	private CTPI_Settings PNL_Settings;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		PNL_PauseMenu = GetNode<Panel>("PNL_PauseMenu");
		BTN_Resume = PNL_PauseMenu.GetNode<CTPI_Button>("MarginContainer/VBoxContainer/BTN_Resume");
		BTN_Options = PNL_PauseMenu.GetNode<CTPI_Button>("MarginContainer/VBoxContainer/BTN_Options");
		BTN_TitleScreen = PNL_PauseMenu.GetNode<CTPI_Button>("MarginContainer/VBoxContainer/BTN_TitleScreen");
		PNL_Settings = GetNode<CTPI_Settings>("PNL_Settings");

		BTN_Resume.Pressed += ResumePressed;
		BTN_Options.Pressed += OptionsPressed;
		BTN_TitleScreen.Pressed += TitleScreenPressed;

		PNL_Settings.ParentMenu = PNL_PauseMenu;
	}

	public new void Show()
	{
		Visible = true;
		Input.MouseMode = Input.MouseModeEnum.Visible;
		Engine.TimeScale = 0;
	}

	private void ResumePressed()
	{
		Visible = false;
		Input.MouseMode = MouseModeWhenResuming;
		Engine.TimeScale = 1;
	}

	private void OptionsPressed()
	{
		PNL_PauseMenu.Visible = false;
		PNL_Settings.Visible = true;
	}

	private void TitleScreenPressed()
	{
		Engine.TimeScale = 1;
		GetTree().ChangeSceneToFile("res://Scenes/UI/S_MainMenu.tscn");
	}
}
