using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class ATPI_TVController : Node
{
	[Export] private RTPI_HolograChapter[] Chapters;

	private VideoStreamPlayer VSP;
	private AudioStreamPlayer3D ASP;
	private int CurrentChapter;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		VSP = GetNode<VideoStreamPlayer>("SubViewport/VideoStreamPlayer");
		ASP = GetNode<AudioStreamPlayer3D>("AudioStreamPlayer3D");

		VSP.Finished += PlayChapter;
		ASP.Finished += PlayChapter;

		CurrentChapter = 0;
		Chapters = Chapters.OrderBy(_ => Guid.NewGuid()).ToArray();
		GD.Print(Chapters);
		PlayChapter();
	}

	private void PlayChapter()
	{
		CurrentChapter++;
		if (CurrentChapter >= Chapters.Length)
			CurrentChapter = 0;

		VSP.Stream = Chapters[CurrentChapter].Video;
		ASP.Stream = Chapters[CurrentChapter].Audio;

		VSP.Play();
		ASP.Play();
	}
}
