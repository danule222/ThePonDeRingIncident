using Godot;
using System;

public partial class ATPI_Evidence : StaticBody3D
{
	[Export] private RTPI_Evidence Evidence;

	public RTPI_Evidence GetEvidence()
	{
		return Evidence;
	}
}
