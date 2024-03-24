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
		this.Position = new Vector2(314, 411);
		GD.Print("Player pos " + this.Position);
	}
	
	private bool nothingPressed = true;

	public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Velocity;

		// Add the gravity.
		if (!nothingPressed) velocity.Y += gravity * (float)delta;

		// Handle Jump inputs.
		if (Input.IsKeyPressed(Key.W) || Input.IsKeyPressed(Key.Space)
			|| Input.IsKeyPressed(Key.Up) || Input.IsMouseButtonPressed(MouseButton.Left)){
				velocity.Y = JumpVelocity;
				if(nothingPressed){
					nothingPressed = false;
					
				} 
			}

		Velocity = velocity;
		MoveAndSlide();


	}

	public void onPlayerDeath(Node2D node){
		Vector2 velocity = Velocity;
		velocity.Y = 0;
		Velocity = velocity;
		GD.Print("Hit Pipe");
		this.Position = new Vector2(314, 411);
		nothingPressed = true;
		GetParent<Node2D>().GetNode<Timer>("spawnClock").Stop();
	}
	public void onPlayerHitFloor(Node2D node){
		Vector2 velocity = Velocity;
		velocity.Y = 0;
		Velocity = velocity;
		GD.Print("Hit Floor");
		this.Position = new Vector2(314,411);
		nothingPressed = true;
		GetParent<Node2D>().GetNode<Timer>("spawnClock").Stop();
	}
	private int score = 0;
	public void onPlayerScore(Node2D body){
		score += 1;
		//GD.Print("Scored: " + score);
		Label scorelabel = GetParent().GetNode<Control>("Control").GetNode<Label>("Label");

		scorelabel.Text = score.ToString();
	}
}
