using Godot;
using System;

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

	private Timer TimeToHideEvidence;

	// Evidence paper
	private Control CON_Evidence;
	private TextureRect IMG_EvidencePhoto;
	private RichTextLabel TXT_EvidenceName;
	private RichTextLabel TXT_EvidenceDescription;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		IMG_Fade = GetNode<TextureRect>("IMG_Fade");
		IMG_Reticle = GetNode<TextureRect>("IMG_Reticle");
		TXT_Time = GetNode<Label>("TXT_Time");
		TXT_Message = GetNode<Label>("TXT_Message");
		TXT_Evidence = GetNode<Label>("TXT_Evidence");

		// Evidence paper
		CON_Evidence = GetNode<Control>("CON_Evidence");
		IMG_EvidencePhoto = CON_Evidence.GetNode<TextureRect>("IMG_EvidencePhoto");
		TXT_EvidenceName = CON_Evidence.GetNode<RichTextLabel>("TXT_EvidenceName");
		TXT_EvidenceDescription = CON_Evidence.GetNode<RichTextLabel>("TXT_EvidenceDescription");
		TimeToHideEvidence = CON_Evidence.GetNode<Timer>("TimeToHideEvidence");

		TimeToHideEvidence.Timeout += HideEvidence;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
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

	private void HideEvidence()
	{
		CON_Evidence.Visible = false;
		TXT_Evidence.Visible = false;
		IMG_Fade.Visible = false;
	}

	public void SetTime(float seconds)
	{
		TimeSpan t = TimeSpan.FromSeconds(seconds);
		TXT_Time.Text = t.ToString(@"m\:ss");
	}
}
