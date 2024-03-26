using Godot;
using System;
using System.Collections.Generic;
using System.Threading;

public partial class gameHandler : Node2D
{

	// An Array that contains the addresses for the pipe nodes
	private List<CanvasGroup> pipeArray = new List<CanvasGroup>(2);
	private Variant dupeScript; // Holds the address to the pipe node script

	private Godot.Timer timer; // timer node declaration

	// Called when the node enters the scene tree for the first time.
	public override void _Ready() {	

		// Initate the score label with 0 as the score
		Label scoreLabel = GetNode<Control>("Control").GetNode<Label>("Label");
		scoreLabel.Text = 0.ToString();

		GetNode<Control>("Control").GetNode<PanelContainer>("Panel").Visible = false;

		// Assign the address for the timer node to the timer variable
		timer = GetNode<Godot.Timer>("spawnClock");
		timer.WaitTime = 1; // Set the count time to 1 second
		
		// Get the inital pipe node and assign it to pipeGroup
		CanvasGroup pipeGroup = GetNode<Node2D>("Pipes").GetNode<CanvasGroup>("CanvasGroup");
		dupeScript = pipeGroup.GetScript(); // Assign dupeScript the address of the script
		pipeArray.Add(pipeGroup); // Add the address of the original pipe to the array
		pipeGroup.SetProcess(false); // Stop the pipe from processing 
	
		// Add the duplicate pipe node to the array
		pipeArray.Add((CanvasGroup)pipeGroup.Duplicate());
		pipeArray[1].SetProcess(false); // Stop the duplicate from processing 
		pipeArray[1].SetScript(dupeScript); // Attach the script to the duplicate
		
		// Play start sound
		GetNode<AudioStreamPlayer>("restartSFX").Play();

		// GD.Print(pipeArray.ToArray()); // Print out the array to see if all nodes were added
	}

	byte releaseIndex = 0; // Index for the pipeArray to be accessed at


	// Signal method for the timer node
	// Is called when timer.WaitTime has finished counting
	// If the timer is stopped, this function won't be called
	private void onTimeout(){
		
		// Switch case that takes in releaseIndex
		switch (releaseIndex) {

			// If the index is 0, start the pipe's process and update the wait time to 2
			case 0:
				pipeArray[releaseIndex].SetProcess(true);
				timer.Stop();
				timer.WaitTime = 2;
				timer.Start();
				break;

			// If the index is 1, add the duplicate pipe, start its process, and stop timer
			case 1:
				AddChild(pipeArray[releaseIndex]);
				pipeArray[releaseIndex].SetProcess(true);
				initPipe(pipeArray[releaseIndex], true);
				timer.Stop();
				GetNode<Control>("Control").GetNode<Label>("Label").TopLevel = true;
				
				break;

			// if the timer does not get stopped at all, it defaults to stopping
			default:
				timer.Stop();
				break;
		}

		releaseIndex++; // Increment to next element
	}


	private float randY; // holds a random value for the Y axis

	// Void function that initates the starting point for the pipe that is passed
	private void initPipe(CanvasGroup pipe, bool onStart){
		randY = GD.RandRange(-200,175) + GD.Randf(); // assign random value
		
		//GD.Print(pipe.Position); // Print starting position before moving

		if(onStart){ // If true is passed, the pipe will start at 0
			pipe.Position = new Vector2(0, randY);

		  // if false the pipe will start closer to the player
		} else pipe.Position = new Vector2(-550, randY);
		
		//GD.Print($"Moved Position {pipe.Position} for pipe {pipe}"); // Print moved position
	}


	// When the pipe is passed, an area behind the player calls this signal method
	private void onPipeRestart(Node2D area){
		
		// Obtains the parent node from the area passed
		CanvasGroup areaParent = (CanvasGroup)area.GetParent();

		/* For each loop that checks if areaParent is equal to 
		   the node in the array and is not the area where the 
		   player's score increments. If so, the pipe is reinitated.
		   False is passed through since the calling of this function
		   means that it has already been initated at 0 */
		foreach (var node in pipeArray){
			if(areaParent == node && area.Name != "ScoreArea"){
				// GD.Print("Restart"); // Print if reached
				if(pipeArray.Contains(areaParent)){
					initPipe(areaParent, false);
				}
				
			}
		}
		
	}

	private bool wasPressed = false; // Logic boolean
	private bool inputs;

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta) {
		
		inputs = Input.IsKeyPressed(Key.W) || 
				 Input.IsKeyPressed(Key.Space) || 
				 Input.IsKeyPressed(Key.Up);

		// If anything is pressed and nothing before it
		// then this means that the game has started
		if(inputs && !wasPressed){
			timer.Start(); // Start timer 
			// GD.Print(timer.WaitTime); // Print starting wait time
			wasPressed = true; // an input was pressed
			initPipe(pipeArray[0], true); // initate the first pipe

			// Hide start instructions
			GetNode<Control>("Control").GetNode<Label>("startLabel").Visible = false;

		}
		
		if(!inputs && !wasPressed){
			// Reset the score back to 0
			score = 0;
			GetNode<Control>("Control").GetNode<Label>("Label").Text = score.ToString();
		}
		
	}

	// score holds the player score
	private int score = 0;

	// When the player passes the ScoreArea node this signal method is called
	public void onPlayerScore(Node2D body){
		score++; // increment score 

		//GD.Print("Scored: " + score); // Print score

		// Update the score label text to the new score
		GetNode<Control>("Control").GetNode<Label>("Label").Text = score.ToString();

		// Play score sound
		GetNode<AudioStreamPlayer>("pointSFX").Play();
	}
	
	// Signal function to stop processes and show menu
	void onPlayerDeath(Node node){
		// Play hit sound
		GetNode<AudioStreamPlayer>("deathSFX").Play();

		// Stop all moving functions (Pipes and Player)
		pipeArray[0].SetProcess(false);
		pipeArray[1].SetProcess(false);
		GetNode<CharacterBody2D>("CharacterBody2D").SetPhysicsProcess(false);

		// Stop the timer
		timer.Stop();
		
		// Reveal menu
		GetNode<Control>("Control").GetNode<PanelContainer>("Panel").TopLevel = true;
		GetNode<Control>("Control").GetNode<PanelContainer>("Panel").Visible = true;

		// Play menu sound
		GetNode<AudioStreamPlayer>("menuSFX").Play();

		// Reset Score
		score = 0;
	}

	// When the restart button is clicked this function is called
	void onRestart(){

		// Start up the player's gravity
		GetNode<CharacterBody2D>("CharacterBody2D").SetPhysicsProcess(true);

		// Get the address for the menu node
		PanelContainer menuPanel = GetNode<Control>("Control").GetNode<PanelContainer>("Panel");

		// Get teh address for the restart button
		Button restartButton = menuPanel.GetNode<VBoxContainer>("VBoxContainer")
									    .GetNode<HBoxContainer>("HBoxContainer")
										.GetNode<Button>("restartBtn");

		// Lose focus of the button
		restartButton.ReleaseFocus();

		// Hide menu
		menuPanel.Visible = false;

		// Revert to orginal start up value
		wasPressed = false;

		// if the dupe pipe was created, remove it 
		if(IsAncestorOf(pipeArray[1])) RemoveChild(pipeArray[1]);

		// Reset Timer and timeout index to orginal values
		releaseIndex = 0;
		timer.WaitTime = 1;

		// Move the orginal pipe back to start
		initPipe(pipeArray[0], true);

		// Reset the score back to 0
		score = 0;
		GetNode<Control>("Control").GetNode<Label>("Label").Text = score.ToString();
		
		// Show start instructions
		GetNode<Control>("Control").GetNode<Label>("startLabel").Visible = true;

		// Play restart sound
		GetNode<AudioStreamPlayer>("restartSFX").Play();

		// Revert to player original position
		GetNode<CharacterBody2D>("CharacterBody2D").Position = new Vector2(314, 411);
	}

	// When the quit button is pressed, this method excutes closing the application
	void onQuit(){
		// Play menu sound
		GetNode<AudioStreamPlayer>("menuSFX").Play();
		
		// Wait for sound to play
		Thread.Sleep(1000);

		// Close app
		GetWindow().GetTree().Quit();
	}

}
