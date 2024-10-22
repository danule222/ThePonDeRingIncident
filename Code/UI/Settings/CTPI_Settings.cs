using Godot;
using System;

public partial class CTPI_Settings : ITPI_Menu
{
	private CTPI_Button BTN_Back;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		BTN_Back = GetNode<CTPI_Button>("HBoxContainer/BTN_Back");

		BTN_Back.Pressed += BackPressed;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
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
