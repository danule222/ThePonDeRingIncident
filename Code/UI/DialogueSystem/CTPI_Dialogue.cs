using Godot;
using GodotInk;
using System;

public partial class CTPI_Dialogue : Control
{
	[Export]
	public InkStory InkStoryTest;
	[Export]
	private PackedScene UI_Button;

	private Control CON_SusPos;
	private Control CON_FwwPos;
	private Control CON_MccPos;

	private Control CON_DialogueBox;
	private RichTextLabel RTL_Text;
	private RichTextLabel RTL_Name;

	private Control CON_FwwSelectionBox;
	private VBoxContainer VBX_FwwOptions;
	private Control CON_MccSelectionBox;
	private VBoxContainer VBX_MccOptions;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		CON_FwwPos = GetNode<Control>("CON_FwwPos");
		CON_MccPos = GetNode<Control>("CON_MccPos");
		CON_SusPos = GetNode<Control>("CON_SusPos");

		CON_DialogueBox = GetNode<Control>("CON_DialogueBox");
		RTL_Text = CON_DialogueBox.GetNode<RichTextLabel>("Panel/MarginContainer/RTL_Text");
		RTL_Name = CON_DialogueBox.GetNode<RichTextLabel>("Panel/PNL_Name/MarginContainer/RTL_Name");

		CON_FwwSelectionBox = GetNode<Control>("CON_FwwSelectionBox");
		VBX_FwwOptions = CON_FwwSelectionBox.GetNode<VBoxContainer>("Panel/MarginContainer/VBX_Options");
		CON_MccSelectionBox = GetNode<Control>("CON_MccSelectionBox");
		VBX_FwwOptions = CON_MccSelectionBox.GetNode<VBoxContainer>("Panel/MarginContainer/VBX_Options");


		Speak(ECharacter.Mococo, "Holaa");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (Input.IsKeyPressed(Key.P))
		{
			// Print text
			if (InkStoryTest.CanContinue)
			{
				GD.Print(InkStoryTest.Continue());
			}
			// Choices
			else if (InkStoryTest.CurrentChoices.Count > 0)
			{
				// Button creation
				foreach (var choice in InkStoryTest.CurrentChoices)
				{
					CTPI_Button button = UI_Button.Instantiate<CTPI_Button>();
					button.Label = choice.Text;
					button.Pressed += delegate ()
					{
						InkStoryTest.ChooseChoiceIndex(choice.Index);
					};

					VBX_FwwOptions.AddChild(button);
				}
			}
		}
	}

	public void Speak(ECharacter character, string text)
	{
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
			default:
				CON_DialogueBox.Position = CON_SusPos.Position;
				break;
		}

		RTL_Text.Text = text; // TODO: Animation
	}
}
