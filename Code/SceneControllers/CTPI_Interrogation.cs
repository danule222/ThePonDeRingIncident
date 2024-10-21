using System;
using System.Text.RegularExpressions;
using Godot;
using Godot.Collections;
using GodotInk;

public partial class CTPI_Interrogation : Control
{
	[Export]
	private Array<RTPI_Interrogation> Interrogations;

	private CTPI_Dialogue UI_Dialogue;
	private InkStory Story;
	private bool Selecting;

	public override void _Ready()
	{
		UI_Dialogue = GetNode<CTPI_Dialogue>("UI_Dialogue");
		UI_Dialogue.Continue += Continue;

		Story = Interrogations[0].Story;
		Selecting = false;

		Continue();
	}

	public override void _Process(double delta)
	{
		if (Input.IsActionJustPressed("dialogic_default_action"))
			Continue();
	}

	private void Continue()
	{
		// Print text
		if (Story.CanContinue)
		{
			string text = Story.Continue();

			// Get Character from text
			string name = Regex.Match(text, "<([^<>]+)>").Groups[1].Value;
			ECharacter character = Enum.Parse<ECharacter>(name);

			// Remove name from text
			text = text.Replace(Regex.Match(text, "<([^<>]+)>").Groups[0].Value + " ", "");

			UI_Dialogue.Speak(character, text);
		}
		// Choices
		else if (Story.CurrentChoices.Count > 0 && !Selecting)
		{
			UI_Dialogue.AddOptions(Story);
			Selecting = true;
		}
	}
}
