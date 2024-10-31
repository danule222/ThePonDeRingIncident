using System;
using System.Text.RegularExpressions;
using Godot;
using Godot.Collections;
using GodotInk;

public partial class CTPI_InterrogationController : Control
{
	[Export]
	private Array<RTPI_Interrogation> Interrogations;
	private CTPI_Dialogue UI_Dialogue;
	private InkStory Story;
	private bool Selecting;
	private bool End;
	private int TensionLevel;
	private int CurrentStory;
	private int CurrentInterrogation;
	private MeshInstance3D MSH_Sus;
	private MeshInstance3D MSH_Fuwawa;
	private MeshInstance3D MSH_Mococo;
	private StandardMaterial3D MAT_Sus;
	private StandardMaterial3D MAT_Fuwawa;
	private StandardMaterial3D MAT_Mococo;

	public override void _Ready()
	{
		Input.MouseMode = Input.MouseModeEnum.Visible;

		UI_Dialogue = GetNode<CTPI_Dialogue>("UI_Dialogue");
		UI_Dialogue.Continue += Continue;
		UI_Dialogue.AddTension += AddTension;

		MSH_Sus = GetNode<MeshInstance3D>("World/MSH_Sus");
		MSH_Fuwawa = GetNode<MeshInstance3D>("World/MSH_Fuwawa");
		MSH_Mococo = GetNode<MeshInstance3D>("World/MSH_Mococo");

		StandardMaterial3D MAT_Sus = MSH_Sus.GetSurfaceOverrideMaterial(0) as StandardMaterial3D;
		StandardMaterial3D MAT_Fuwawa = MSH_Fuwawa.GetSurfaceOverrideMaterial(0) as StandardMaterial3D;
		StandardMaterial3D MAT_Mococo = MSH_Mococo.GetSurfaceOverrideMaterial(0) as StandardMaterial3D;
		// Set textures
		MAT_Sus.AlbedoTexture = Interrogations[0].Character.Emotions[EEmotion.Neutral];
		MAT_Fuwawa.AlbedoTexture = Interrogations[0].Character.Emotions[EEmotion.Neutral];
		MAT_Mococo.AlbedoTexture = Interrogations[0].Character.Emotions[EEmotion.Neutral];

		CurrentStory = 0;
		CurrentInterrogation = 0;
		Story = Interrogations[CurrentInterrogation].Stories[CurrentStory];
		TensionLevel = Interrogations[CurrentInterrogation].TensionLevelStart;
		Selecting = false;
		End = false;

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
			if (Selecting) Selecting = false;

			string text = Story.Continue();

			Match rgx = Regex.Match(text, "<([^:<>]+):([^:<>]+):([^:<>]+)>");
			// Get Character from text
			string name = rgx.Groups[1].Value;
			ECharacter character = Enum.Parse<ECharacter>(name);
			// Get Emotion from text
			string emotion_str = rgx.Groups[2].Value;
			EEmotion emotion = Enum.Parse<EEmotion>(emotion_str);

			// Remove name from text
			text = text.Replace(rgx.Groups[0].Value + " ", "");

			UI_Dialogue.Speak(character, text);
		}
		// Choices
		else if (Story.CurrentChoices.Count > 0 && !Selecting)
		{
			UI_Dialogue.AddOptions(Story);
			Selecting = true;
		}
		// Change story
		else if (!Selecting && !End)
		{
			ChangeStory();
		}
	}

	private void AddTension(int tension)
	{
		TensionLevel += tension;
	}

	private void ChangeStory()
	{
		switch (CurrentStory)
		{
			// Introduction
			case 0:
				if (TensionLevel > 0)
					CurrentStory = 1;
				else
					CurrentStory = 2;
				break;
			case 1:
				if (TensionLevel > 0)
					CurrentStory = 3;
				else
					CurrentStory = 4;
				break;
			case 2:
				if (TensionLevel > 0)
					CurrentStory = 4;
				else
					CurrentStory = 5;
				break;
			default:
				ChangeInterrogation();
				return;
		}

		Story = Interrogations[CurrentInterrogation].Stories[CurrentStory];
		Story.ResetState(); // Just in case is a repeated story
		Continue();
	}

	private void ChangeInterrogation()
	{
		CurrentInterrogation++;
		if (CurrentInterrogation >= Interrogations.Count)
		{
			End = true;
			GD.Print("End");
			return;
		}

		CurrentStory = 0;
		Story = Interrogations[CurrentInterrogation].Stories[CurrentStory];
		Story.ResetState(); // Just in case is a repeated story
		TensionLevel = Interrogations[CurrentInterrogation].TensionLevelStart;

		Continue();
	}
}
