using Godot;

public partial class ATPI_PlayerController : CharacterBody3D
{
	[ExportCategory("Character")]
	[Export]
	private float baseSpeed = 3.0f;
	[Export]
	private float sprintSpeed = 6.0f;
	[Export]
	private float crouchSpeed = 1.0f;

	[Export]
	private float acceleration = 10.0f;
	[Export]
	private float jumpVelocity = 4.5f;
	[Export]
	private float mouseSensitivity = 0.005f;
	[Export]
	private bool immobile = false;

	[ExportGroup("Nodes")]
	[Export]
	Camera3D CAMERA;

	// Member variables
	private float speed;
	private float currentSpeed = 0.0f;
	private bool lowCeiling = false; // This is for when the ceiling is too low and the player needs to crouch.
	private bool wasOnFloor = true;

	private RayCast3D RayCast;
	private bool ColliderDetected = false;
	private ATPI_Evidence LastEvidence;
	private ATPI_InvestigationController InvestigationController;

	public override void _Ready()
	{
		speed = baseSpeed;

		InvestigationController = GetParent<ATPI_InvestigationController>();
		RayCast = GetNode<RayCast3D>("Camera3D/RayCast3D");
	}

	public override void _Process(double delta)
	{
		currentSpeed = Vector3.Zero.DistanceTo(GetRealVelocity());

		Vector2 inputDir = immobile ? Vector2.Zero : Input.GetVector("move_left", "move_right", "move_forward", "move_backwards");

		HandleMovement(delta, inputDir);
		HandleRayCast();
	}

	private void HandleMovement(double delta, Vector2 inputDir)
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

	private void HandleRayCast()
	{
		GodotObject obj = RayCast.GetCollider();
		if (obj != null && !ColliderDetected)
		{
			ATPI_Evidence evidence = obj as ATPI_Evidence;
			if (evidence != null)
			{
				ColliderDetected = true;

				if (evidence != LastEvidence)
				{
					InvestigationController.HUD.SetReticle(true);

					LastEvidence = evidence;
				}
			}
		}
		else if (obj != LastEvidence && ColliderDetected)
		{
			InvestigationController.HUD.SetReticle(false);

			ColliderDetected = false;
			LastEvidence = null;
		}
	}

	public override void _Input(InputEvent @event)
	{
		if (@event is InputEventMouseMotion && Input.MouseMode == Input.MouseModeEnum.Captured)
		{
			InputEventMouseMotion iemm = (InputEventMouseMotion)@event;
			Vector3 currentRotation = CAMERA.Rotation;
			currentRotation.Y -= iemm.Relative.X * mouseSensitivity;
			currentRotation.X -= iemm.Relative.Y * mouseSensitivity;

			currentRotation.X = Mathf.Clamp(currentRotation.X, -1.5708f, 1.5708f);

			CAMERA.Rotation = currentRotation;
		}

		if (@event.IsActionPressed("dialogic_default_action") && LastEvidence != null)
			InvestigationController.AddEvidence(LastEvidence.GetEvidence());

		if (@event.IsActionPressed("pause"))
			InvestigationController.Pause();
	}
}
