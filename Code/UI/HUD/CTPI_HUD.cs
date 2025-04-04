using Godot;
using System;
using System.Reflection;

public partial class CTPI_HUD : Control
{
	[Export]
	private Texture2D ReticleDefault;
	[Export]
	private Texture2D ReticleEvidence;

	private TextureRect IMG_Fade;
	private TextureRect IMG_Reticle;
	private Label TXT_Time;
	private Label TXT_Message;
	private Label TXT_Evidence;
	private Label TXT_Evidences;

	private Timer TimeToHideEvidence;
	private Timer TimeToMessage;
	private Timer MessageDelay;

	// Evidence paper
	private Control CON_Evidence;
	private TextureRect IMG_EvidencePhoto;
	private RichTextLabel TXT_EvidenceName;
	private RichTextLabel TXT_EvidenceDescription;

	private Action DelayMethod;
	public CTPI_PauseMenu UI_PauseMenu { get; private set; }

	// Instructions
	private Panel PNL_Instructions;
	private CTPI_Button BTN_Start;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Input.MouseMode = Input.MouseModeEnum.Visible;

		IMG_Fade = GetNode<TextureRect>("IMG_Fade");
		IMG_Reticle = GetNode<TextureRect>("IMG_Reticle");
		TXT_Time = GetNode<Label>("TXT_Time");
		TXT_Message = GetNode<Label>("TXT_Message");
		TXT_Evidence = GetNode<Label>("TXT_Evidence");
		TXT_Evidences = GetNode<Label>("TXT_Evidences");
		TimeToMessage = GetNode<Timer>("TimeToMessage");
		MessageDelay = GetNode<Timer>("MessageDelay");

		// Evidence paper
		CON_Evidence = GetNode<Control>("CON_Evidence");
		IMG_EvidencePhoto = CON_Evidence.GetNode<TextureRect>("IMG_EvidencePhoto");
		TXT_EvidenceName = CON_Evidence.GetNode<RichTextLabel>("TXT_EvidenceName");
		TXT_EvidenceDescription = CON_Evidence.GetNode<RichTextLabel>("TXT_EvidenceDescription");
		TimeToHideEvidence = CON_Evidence.GetNode<Timer>("TimeToHideEvidence");

		TimeToHideEvidence.Timeout += HideEvidence;
		TimeToMessage.Timeout += HideMessage;

		UI_PauseMenu = GetNode<CTPI_PauseMenu>("UI_PauseMenu");

		// Instructions
		PNL_Instructions = GetNode<Panel>("PNL_Instructions");
		BTN_Start = PNL_Instructions.GetNode<CTPI_Button>("BTN_Start");

		BTN_Start.Pressed += StartInvestigation;

		DelayMethod = delegate { GD.Print("owo"); };
		MessageDelay.Timeout += DelayMethod;

		Engine.TimeScale = 0;
		PNL_Instructions.Visible = true;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private void StartInvestigation()
	{
		Engine.TimeScale = 1;
		PNL_Instructions.Visible = false;
		Input.MouseMode = Input.MouseModeEnum.Captured;
	}

	public void SetReticle(bool isEvidence)
	{
		if (isEvidence)
			IMG_Reticle.Texture = ReticleEvidence;
		else
			IMG_Reticle.Texture = ReticleDefault;
	}

	public void SetEvidence(RTPI_Evidence evidence)
	{
		CON_Evidence.Visible = true;
		TXT_Evidence.Visible = true;
		IMG_Fade.Visible = true;

		IMG_EvidencePhoto.Texture = evidence.Photo;
		TXT_EvidenceName.Text = "[center]" + evidence.Name + "[/center]";
		TXT_EvidenceDescription.Text = evidence.Description;

		TimeToHideEvidence.Start();
	}

	public void AddEvidence(int found, int total)
	{
		TXT_Evidences.Text = found + "/" + total;
	}

	private void HideEvidence()
	{
		CON_Evidence.Visible = false;
		TXT_Evidence.Visible = false;
		IMG_Fade.Visible = false;
	}

	public void SetMessage(string message, int delay = 0)
	{
		if (delay > 0 || TimeToMessage.TimeLeft > 0 || TimeToHideEvidence.TimeLeft > 0)
		{
			MessageDelay.Timeout -= DelayMethod;
			DelayMethod = delegate
			{
				SetMessage(message, 0);
			};
			MessageDelay.Timeout += DelayMethod;
			MessageDelay.WaitTime = delay + TimeToMessage.TimeLeft + TimeToHideEvidence.TimeLeft;
			MessageDelay.Start();

			return;
		}

		TXT_Message.Visible = true;
		IMG_Fade.Visible = true;

		TXT_Message.Text = message;
		TimeToMessage.Start();
	}

	private void HideMessage()
	{
		TXT_Message.Visible = false;
		IMG_Fade.Visible = false;
	}

	public void SetTime(float seconds)
	{
		TimeSpan t = TimeSpan.FromSeconds(seconds);
		TXT_Time.Text = t.ToString(@"m\:ss");
	}

	public void ShowPauseMenu()
	{
		if (!PNL_Instructions.Visible)
			UI_PauseMenu.Show();
	}
}
