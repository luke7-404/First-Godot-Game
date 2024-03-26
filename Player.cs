using Godot;
using System;

public partial class Player : CharacterBody2D
{
	
	
	// Preset Godot Physics Variables
	public const float JumpVelocity = -400.0f;

	// Get the gravity from the project settings to be synced with RigidBody nodes.
	public float gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();

	// Called when the node enters the scene tree for the first time.
	public override void _Ready(){

		// Initate the player with a set position 
		this.Position = new Vector2(314, 411);

		// GD.Print("Player pos " + this.Position); // Print if player's position moved
	}
	
	private bool nothingPressed = true;	// A logic boolean

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(double delta){

		// Godot Code
		Vector2 velocity = Velocity;

		// If an input was received add the gravity
		if (!nothingPressed) velocity.Y += gravity * (float)delta;

		// Handle Jump inputs.
		if (Input.IsKeyPressed(Key.W) || Input.IsKeyPressed(Key.Space)
			|| Input.IsKeyPressed(Key.Up) || Input.IsMouseButtonPressed(MouseButton.Left)){
				velocity.Y = JumpVelocity;

				// If reached, something was pressed
				if(nothingPressed) nothingPressed = false;
					
			}

		// Godot Code
		Velocity = velocity;
		MoveAndSlide();


	}

	// Signal function to reset player if a pipe was hit
	public void onPlayerDeath(Node2D node){
		// Reset Y position
		Vector2 velocity = Velocity;
		velocity.Y = 0;
		Velocity = velocity;

		/* print death cause
		GD.Print("Hit Pipe");  
		*/

		// Revert to player original position
		Position = new Vector2(314, 411);
		
		nothingPressed = true; // Stop Gravity

		// Place holder for a reset button
		GetWindow().GetTree().Paused = true;
	}
	
	// score holds the player score
	private int score = 0;

	// When the player passes the ScoreArea node this signal method is called
	public void onPlayerScore(Node2D body){
		score++; // increment score 

		//GD.Print("Scored: " + score); // Print score

		// Get the adress of the score label node
		Label scoreLabel = GetParent().GetNode<Control>("Control").GetNode<Label>("Label");

		// Update the score label text to the new score
		scoreLabel.Text = score.ToString();
	}
}
