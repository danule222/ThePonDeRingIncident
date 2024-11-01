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
	[Export]
	public int LabelSize
	{
		get => _LabelSize;
		set
		{
			_LabelSize = value;
			OnLabelSizeSet();
		}
	}
	[Signal] public delegate void PressedEventHandler();

	private string _LabelString;
	private int _LabelSize;
	private Button _Button;

	private void OnButtonSet()
	{
		if (_LabelString.Length > 0)
			GetNode<Label>("Button/MarginContainer/Label").Text = _LabelString;
		else
			GetNode<Label>("Button/MarginContainer/Label").Text = "Label";
	}

	private void OnLabelSizeSet()
	{
		Label l = GetNodeOrNull<Label>("Button/MarginContainer/Label");

		if (l != null)
			l.Set("theme_override_font_sizes/font_size", _LabelSize);
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
