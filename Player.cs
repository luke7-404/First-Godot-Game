using Godot;
using System;

public partial class Player : CharacterBody2D
{
	public const float Speed = 300.0f;
	public const float JumpVelocity = -400.0f;

	// Get the gravity from the project settings to be synced with RigidBody nodes.
	public float gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();

    public override void _Ready()
    {
        var child = this.GetNode<Sprite2D>("Sprite2D");
		GD.Print(child);
		child.Rotate(30);
    }

    public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Velocity;

		// Add the gravity.
		/*if (!IsOnFloor())
			velocity.Y += gravity * (float)delta;*/

		// Handle Jump.
		if (Input.IsActionJustPressed("ui_accept") && IsOnFloor())
			velocity.Y = JumpVelocity;

		// Get the input direction and handle the movement/deceleration.
		// As good practice, you should replace UI actions with custom gameplay actions.
		
		if (Input.IsKeyPressed(Key.W)) this.Position += new Vector2(0, -5.0f);
		if (Input.IsKeyPressed(Key.A)) this.Position += new Vector2(-5.0f, 0);
		if (Input.IsKeyPressed(Key.S)) this.Position += new Vector2(0, 5.0f);
		if (Input.IsKeyPressed(Key.D)) this.Position += new Vector2(5.0f, 0);

		Velocity = velocity;
		MoveAndSlide();
	}
}
