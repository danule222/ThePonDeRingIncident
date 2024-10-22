using Godot;

public partial class ATPI_PlayerController : CharacterBody3D
{
	[ExportCategory("Character")]
	[Export]
	float baseSpeed = 3.0f;
	[Export]
	float sprintSpeed = 6.0f;
	[Export]
	float crouchSpeed = 1.0f;

	[Export]
	float acceleration = 10.0f;
	[Export]
	float jumpVelocity = 4.5f;
	[Export]
	float mouseSensitivity = 0.005f;
	[Export]
	bool immobile = false;

	[ExportGroup("Nodes")]
	[Export]
	Camera3D CAMERA;

	// Member variables
	float speed;
	float currentSpeed = 0.0f;
	bool lowCeiling = false; // This is for when the ceiling is too low and the player needs to crouch.
	bool wasOnFloor = true;

	public override void _Ready()
	{
		base._Ready();
		speed = baseSpeed;

		Input.MouseMode = Input.MouseModeEnum.Captured;
	}

	public override void _Process(double delta)
	{
		currentSpeed = Vector3.Zero.DistanceTo(GetRealVelocity());

		Vector2 inputDir = immobile ? Vector2.Zero : Input.GetVector("move_left", "move_right", "move_forward", "move_backwards");

		HandleMovement(delta, inputDir);
	}

	void HandleMovement(double delta, Vector2 inputDir)
	{
		Vector2 direction2D = inputDir.Rotated(-CAMERA.Rotation.Y);
		Vector3 direction = new Vector3(direction2D.X, 0, direction2D.Y);
		direction = direction.Normalized();

		Vector3 currentVelocity = Vector3.Zero;
		currentVelocity.X = Mathf.Lerp(Velocity.X, direction.X * speed, (float)(acceleration * delta));
		currentVelocity.Z = Mathf.Lerp(Velocity.Z, direction.Z * speed, (float)(acceleration * delta));
		Velocity = currentVelocity;

		MoveAndSlide();
	}

	public override void _UnhandledInput(InputEvent @event)
	{
		if (@event is InputEventMouseMotion && Input.MouseMode == Input.MouseModeEnum.Captured)
		{
			InputEventMouseMotion iemm = (InputEventMouseMotion)@event;
			Vector3 currentRotation = CAMERA.Rotation;
			currentRotation.Y -= iemm.Relative.X * mouseSensitivity;
			currentRotation.X -= iemm.Relative.Y * mouseSensitivity;
			CAMERA.Rotation = currentRotation;
		}
	}
}
