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
	private Control CON_NarPos;

	private Control CON_DialogueBox;
	private RichTextLabel RTL_Text;
	private RichTextLabel RTL_Name;

	private Control CON_FwwSelectionBox;
	private VBoxContainer VBX_FwwOptions;
	private Control CON_MccSelectionBox;
	private VBoxContainer VBX_MccOptions;
	[Signal] public delegate void ContinueEventHandler();
	[Signal] public delegate void AddTensionEventHandler(int tension);

	private Timer NewCharacter;
	public bool IsSpeaking { get; private set; }
	private string Text;
	private int CurrentCharacter;

	public override void _Ready()
	{
		CON_FwwPos = GetNode<Control>("CON_FwwPos");
		CON_MccPos = GetNode<Control>("CON_MccPos");
		CON_SusPos = GetNode<Control>("CON_SusPos");
		CON_NarPos = GetNode<Control>("CON_NarPos");

		CON_DialogueBox = GetNode<Control>("CON_DialogueBox");
		RTL_Text = CON_DialogueBox.GetNode<RichTextLabel>("Panel/MarginContainer/RTL_Text");
		RTL_Name = CON_DialogueBox.GetNode<RichTextLabel>("Panel/PNL_Name/MarginContainer/RTL_Name");

		CON_FwwSelectionBox = GetNode<Control>("CON_FwwSelectionBox");
		VBX_FwwOptions = CON_FwwSelectionBox.GetNode<VBoxContainer>("Panel/MarginContainer/VBX_Options");
		CON_MccSelectionBox = GetNode<Control>("CON_MccSelectionBox");
		VBX_MccOptions = CON_MccSelectionBox.GetNode<VBoxContainer>("Panel/MarginContainer/VBX_Options");

		NewCharacter = GetNode<Timer>("NewCharacter");
		NewCharacter.Timeout += TypeWriteEffect;
		RTL_Text.Text = "";
	}

	public override void _Process(double delta)
	{
	}

	public void Speak(ECharacter character, string text)
	{
		NewCharacter.Stop();
		IsSpeaking = true;
		CurrentCharacter = 0;

		// Visibility
		CON_DialogueBox.Visible = true;

		// Name
		RTL_Name.Text = character.ToString();

		// Position
		switch (character)
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
			default:
				CON_DialogueBox.Position = CON_SusPos.Position;
				break;
		}

		Text = text;
		RTL_Text.Text = "";
		NewCharacter.Start();
	}

	public void Skip()
	{
		IsSpeaking = false;
		NewCharacter.Stop();
		RTL_Text.Text = Text;
	}

	private void TypeWriteEffect()
	{
		if (CurrentCharacter + 1 < Text.Length)
		{
			RTL_Text.Text = Text.Substr(0, CurrentCharacter + 1)
				+ "[color=#00000000]" + Text.Substring(CurrentCharacter) + "[/color]";

			// RTL_Text.Text += Text[CurrentCharacter];
			CurrentCharacter++;
		}
		else
		{
			IsSpeaking = false;
			NewCharacter.Stop();
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
}
