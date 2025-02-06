using System;
using System.Text.RegularExpressions;
using Godot;
using GodotInk;

public partial class CTPI_Dialogue : Control
{
	[Export]
	private PackedScene UI_Button;

	private Control CON_SusPos;
	private Control CON_FwwPos;
	private Control CON_MccPos;
	private Control CON_FWMCPos;
	private Control CON_NarPos;

	private Control CON_DialogueBox;
	private RichTextLabel RTL_Text;
	private RichTextLabel RTL_Name;

	private RTPI_Character CurrentCharacter;
	private double VoiceBasePitch;

	private Control CON_FwwSelectionBox;
	private VBoxContainer VBX_FwwOptions;
	private Control CON_MccSelectionBox;
	private VBoxContainer VBX_MccOptions;
	[Signal] public delegate void ContinueEventHandler();
	[Signal] public delegate void AddTensionEventHandler(int tension);

	private Timer NewTextCharacter;
	public bool IsSpeaking { get; private set; }
	private string Text;
	private int CurrentTextCharacter;
	private AudioStreamPlayer VoicePlayer;
	private double CurrentWaitTime;

	private CTPI_PauseMenu UI_PauseMenu;

	// End panel
	private Panel PNL_End;
	private Panel PNL_Wrong;
	private Panel PNL_Correct;
	private CTPI_Button BTN_Baelz;
	private CTPI_Button BTN_Gura;
	private CTPI_Button BTN_Ollie;
	private CTPI_Button BTN_Ina;
	private CTPI_Button BTN_Korone;
	private Timer TimeToMenu;
	private bool End;

	public override void _Ready()
	{
		CON_FwwPos = GetNode<Control>("CON_FwwPos");
		CON_MccPos = GetNode<Control>("CON_MccPos");
		CON_FWMCPos = GetNode<Control>("CON_FWMCPos");
		CON_SusPos = GetNode<Control>("CON_SusPos");
		CON_NarPos = GetNode<Control>("CON_NarPos");

		CON_DialogueBox = GetNode<Control>("CON_DialogueBox");
		RTL_Text = CON_DialogueBox.GetNode<RichTextLabel>("Panel/MarginContainer/RTL_Text");
		RTL_Name = CON_DialogueBox.GetNode<RichTextLabel>("Panel/PNL_Name/MarginContainer/RTL_Name");

		CON_FwwSelectionBox = GetNode<Control>("CON_FwwSelectionBox");
		VBX_FwwOptions = CON_FwwSelectionBox.GetNode<VBoxContainer>("Panel/MarginContainer/VBX_Options");
		CON_MccSelectionBox = GetNode<Control>("CON_MccSelectionBox");
		VBX_MccOptions = CON_MccSelectionBox.GetNode<VBoxContainer>("Panel/MarginContainer/VBX_Options");

		UI_PauseMenu = GetNode<CTPI_PauseMenu>("UI_PauseMenu");

		PNL_End = GetNode<Panel>("PNL_End");
		BTN_Baelz = PNL_End.GetNode<CTPI_Button>("VBoxContainer/BTN_Baelz");
		BTN_Gura = PNL_End.GetNode<CTPI_Button>("VBoxContainer/BTN_Gura");
		BTN_Ollie = PNL_End.GetNode<CTPI_Button>("VBoxContainer/BTN_Ollie");
		BTN_Ina = PNL_End.GetNode<CTPI_Button>("VBoxContainer/BTN_Ina");
		BTN_Korone = PNL_End.GetNode<CTPI_Button>("VBoxContainer/BTN_Korone");
		PNL_Wrong = GetNode<Panel>("PNL_Wrong");
		PNL_Correct = GetNode<Panel>("PNL_Correct");
		TimeToMenu = GetNode<Timer>("TimeToMenu");
		End = false;

		BTN_Baelz.Pressed += WrongAnswer;
		BTN_Ollie.Pressed += WrongAnswer;
		BTN_Ina.Pressed += WrongAnswer;
		BTN_Korone.Pressed += WrongAnswer;
		BTN_Gura.Pressed += CorrectAnswer;

		TimeToMenu.Timeout += ToTitleMenu;

		NewTextCharacter = GetNode<Timer>("NewTextCharacter");
		NewTextCharacter.Timeout += TypeWriteEffect;
		RTL_Text.Text = "";
		VoicePlayer = GetNode<AudioStreamPlayer>("VoicePlayer");
	}

	public override void _Process(double delta)
	{
	}

	public void Speak(RTPI_Character character, EEmotion emotion, string text)
	{
		NewTextCharacter.Stop();
		IsSpeaking = true;
		CurrentTextCharacter = 0;

		CurrentCharacter = character;

		// Visibility
		CON_DialogueBox.Visible = true;

		// Name
		RTL_Name.Text = character.Name.ToString();

		// Position
		switch (character.Name)
		{
			case ECharacter.Fuwawa:
				CON_DialogueBox.Position = CON_FwwPos.Position;
				break;
			case ECharacter.Mococo:
				CON_DialogueBox.Position = CON_MccPos.Position;
				break;
			case ECharacter.Narrator:
				CON_DialogueBox.Position = CON_NarPos.Position;
				break;
			case ECharacter.FWMC:
				CON_DialogueBox.Position = CON_FWMCPos.Position;
				break;
			default:
				CON_DialogueBox.Position = CON_SusPos.Position;
				break;
		}

		// Emotion
		switch (emotion)
		{
			case EEmotion.Neutral:
				VoiceBasePitch = 1.0;
				CurrentWaitTime = 0.1;
				break;
			case EEmotion.Happy:
				VoiceBasePitch = 1.1;
				CurrentWaitTime = 0.1;
				break;
			case EEmotion.Alert:
				VoiceBasePitch = 1.1;
				CurrentWaitTime = 0.08;
				break;
			case EEmotion.Thinking:
				VoiceBasePitch = 0.9;
				CurrentWaitTime = 0.12;
				break;
			case EEmotion.Sad:
				VoiceBasePitch = 0.9;
				CurrentWaitTime = 0.12;
				break;
			case EEmotion.Mad:
				VoiceBasePitch = 1.1;
				CurrentWaitTime = 0.05;
				break;
			case EEmotion.Laugh:
				VoiceBasePitch = 1.2;
				CurrentWaitTime = 1.0;
				break;
		}
		NewTextCharacter.WaitTime = CurrentWaitTime;

		Text = text;
		RTL_Text.Text = "";
		NewTextCharacter.Start();
	}

	public void Skip()
	{
		IsSpeaking = false;
		NewTextCharacter.Stop();
		RTL_Text.Text = Text;
	}

	private void TypeWriteEffect()
	{
		if (CurrentTextCharacter + 1 < Text.Length)
		{
			RTL_Text.Text = Text.Substr(0, CurrentTextCharacter + 1)
				+ "[color=#00000000]" + Text.Substring(CurrentTextCharacter) + "[/color]";

			CurrentTextCharacter++;

			// Sound voice
			if (
					Text[CurrentTextCharacter] != ' ' &&
					Text[CurrentTextCharacter] != ',' &&
					Text[CurrentTextCharacter] != '.' &&
					Text[CurrentTextCharacter] != '?' &&
					Text[CurrentTextCharacter] != '!' &&
					Text[CurrentTextCharacter] != '\n')
			{
				VoicePlayer.Stream = CurrentCharacter.Voice;
				VoicePlayer.PitchScale = (float)GD.RandRange(VoiceBasePitch - 0.1, VoiceBasePitch + 0.1);
				VoicePlayer.Play();
			}

			// Stops and commas
			if (
				Text[CurrentTextCharacter] == ',' ||
				Text[CurrentTextCharacter] == '.')
			{
				NewTextCharacter.WaitTime = CurrentWaitTime + 0.3;
			}
			else
			{
				NewTextCharacter.WaitTime = CurrentWaitTime;
			}
		}
		else
		{
			IsSpeaking = false;
			NewTextCharacter.Stop();
		}
	}

	public void AddOptions(InkStory story)
	{
		foreach (var child in VBX_FwwOptions.GetChildren())
			child.QueueFree();
		foreach (var child in VBX_MccOptions.GetChildren())
			child.QueueFree();

		CON_FwwSelectionBox.Visible = true;
		CON_MccSelectionBox.Visible = true;

		// Button creation
		foreach (var choice in story.CurrentChoices)
		{
			string text = choice.Text;
			// Name
			string name = Regex.Match(text, "<([^:<>]+):([^:<>]+):([^:<>]+)>").Groups[1].Value;
			ECharacter character = Enum.Parse<ECharacter>(name);
			// Character
			int tension = Regex.Match(text, "<([^:<>]+):([^:<>]+):([^:<>]+)>").Groups[3].Value.ToInt();
			// Text
			text = text.Replace(Regex.Match(text, "<([^:<>]+):([^:<>]+):([^:<>]+)>").Groups[0].Value + " ", "");

			CTPI_Button button = UI_Button.Instantiate<CTPI_Button>();
			button.Label = text;
			button.LabelSize = 22;
			button.Pressed += delegate ()
			{
				story.ChooseChoiceIndex(choice.Index);
				CON_FwwSelectionBox.Visible = false;
				CON_MccSelectionBox.Visible = false;

				EmitSignal("Continue");
				EmitSignal("AddTension", tension);
			};

			if (character == ECharacter.Fuwawa)
				VBX_FwwOptions.AddChild(button);
			else
				VBX_MccOptions.AddChild(button);
		}
	}

	public bool IsPaused() => UI_PauseMenu.Visible;

	public void Pause()
	{
		if (!End)
			UI_PauseMenu.Show();
	}

	public void ShowEnd()
	{
		End = true;
		PNL_End.Visible = true;
		CON_DialogueBox.Visible = false;
		CON_FwwSelectionBox.Visible = false;
		CON_MccSelectionBox.Visible = false;
	}

	private void WrongAnswer()
	{
		PNL_End.Visible = false;
		PNL_Wrong.Visible = true;
		TimeToMenu.Start();
	}

	private void CorrectAnswer()
	{
		PNL_End.Visible = false;
		PNL_Correct.Visible = true;
		TimeToMenu.Start();
	}

	private void ToTitleMenu()
	{
		Engine.TimeScale = 1;
		GetTree().ChangeSceneToFile("res://Scenes/UI/S_MainMenu.tscn");
	}
}
