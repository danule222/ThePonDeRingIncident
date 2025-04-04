using System;
using System.Linq;
using System.Text.RegularExpressions;
using Godot;
using Godot.Collections;
using GodotInk;

public partial class CTPI_InterrogationController : Control
{
	[Export]
	private Array<RTPI_Interrogation> Interrogations;
	[Export]
	private Array<RTPI_Character> Characters;
	[Export(PropertyHint.NodeType)]
	private Dictionary<RTPI_Character, NodePath> CharacterMesh;
	private Dictionary<ECharacter, Node3D> CharacterNodes;
	private Dictionary<ECharacter, RTPI_Character> EnumCharacter;
	private CTPI_Dialogue UI_Dialogue;
	private InkStory Story;
	private bool Selecting;
	private bool End;
	private int TensionLevel;
	private int CurrentStory;
	private int CurrentInterrogation;

	public override void _Ready()
	{
		Input.MouseMode = Input.MouseModeEnum.Visible;

		UI_Dialogue = GetNode<CTPI_Dialogue>("UI_Dialogue");
		UI_Dialogue.Continue += Continue;
		UI_Dialogue.AddTension += AddTension;

		CurrentStory = 0;
		CurrentInterrogation = 0;
		Story = Interrogations[CurrentInterrogation].Stories[CurrentStory];
		TensionLevel = Interrogations[CurrentInterrogation].TensionLevelStart;
		Selecting = false;
		End = false;

		CharacterNodes = new Dictionary<ECharacter, Node3D>();
		for (int i = 0; i < CharacterMesh.Count; i++)
		{
			CharacterNodes.Add(
				CharacterMesh.Keys.ElementAt(i).Name,
				GetNode<Node3D>(CharacterMesh.Values.ElementAt(i)));
		}

		CharacterNodes[Interrogations[CurrentInterrogation].Character.Name].Visible = true;

		EnumCharacter = new Dictionary<ECharacter, RTPI_Character>();
		foreach (RTPI_Character c in Characters)
		{
			EnumCharacter.Add(c.Name, c);
		}

		Continue();

		// Music
		NTPI_AL_AudioController.Instance.PlayBackgroundMusic(EGameState.Interrogation);
	}

	public override void _Process(double delta)
	{
		if (Input.IsActionJustPressed("dialogic_default_action") && !UI_Dialogue.IsPaused())
			Continue();

		if (Input.IsActionJustPressed("pause") && !UI_Dialogue.IsPaused())
		{
			UI_Dialogue.Pause();
		}
	}

	private void Continue()
	{
		if (UI_Dialogue.IsSpeaking)
		{
			UI_Dialogue.Skip();
			return;
		}

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

			UI_Dialogue.Speak(EnumCharacter[character], emotion, text);

			if (text.Contains("exits the room") || text.Contains("leaves the room"))
				CharacterNodes[Interrogations[CurrentInterrogation].Character.Name].Visible = false;
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
		CharacterNodes[Interrogations[CurrentInterrogation].Character.Name].Visible = false;

		CurrentInterrogation++;
		if (CurrentInterrogation >= Interrogations.Count)
		{
			EndInterrogation();
			return;
		}

		CurrentStory = 0;
		Story = Interrogations[CurrentInterrogation].Stories[CurrentStory];
		Story.ResetState(); // Just in case is a repeated story
		TensionLevel = Interrogations[CurrentInterrogation].TensionLevelStart;

		CharacterNodes[Interrogations[CurrentInterrogation].Character.Name].Visible = true;

		Continue();
	}

	private void EndInterrogation()
	{
		End = true;
		UI_Dialogue.ShowEnd();
	}
}
