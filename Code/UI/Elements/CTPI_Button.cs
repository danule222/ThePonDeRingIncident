using Godot;
using System;

[GlobalClass, Tool]
public partial class CTPI_Button : Control
{
	[Export]
	public string Label
	{
		get => _LabelString;
		set
		{
			_LabelString = value;
			OnButtonSet();
		}
	}
	[Signal] public delegate void PressedEventHandler();

	private Label _Label;
	private string _LabelString;
	private Button _Button;

	private void OnButtonSet()
	{
		if (_LabelString.Length > 0)
			GetNode<Label>("Button/MarginContainer/Label").Text = _LabelString;
		else
			GetNode<Label>("Button/MarginContainer/Label").Text = "Label";
	}

	private void OnButtonPressed()
	{
		EmitSignal("Pressed");
	}

	public override void _Ready()
	{
		GetNode<Button>("Button").Pressed += OnButtonPressed;
	}
}
