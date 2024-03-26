using Godot;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

public partial class gameHandler : Node2D
{

	// An Array that contains the addresses for the pipe nodes
	private List<CanvasGroup> pipeArray = new List<CanvasGroup>(2);
	private Variant dupeScript; // Holds the address to the pipe node script

	private Timer timer; // timer node declaration

	// Called when the node enters the scene tree for the first time.
	public override void _Ready() {	

		// Initate the score label with 0 as the score
		Label scoreLabel = GetNode<Control>("Control").GetNode<Label>("Label");
		scoreLabel.Text = 0.ToString();

		// Assign the address for the timer node to the timer variable
		timer = GetNode<Timer>("spawnClock");
		timer.WaitTime = 1; // Set the count time to 1 second
		
		// Get the inital pipe node and assign it to pipeGroup
		CanvasGroup pipeGroup = (CanvasGroup)GetNode<Node2D>("Pipes").GetNode<CanvasGroup>("CanvasGroup");
		dupeScript = pipeGroup.GetScript(); // Assign dupeScript the address of the script
		pipeArray.Add(pipeGroup); // Add the address of the original pipe to the array
		pipeGroup.SetProcess(false); // Stop the pipe from processing 
	
		// Add the duplicate pipe node to the array
		pipeArray.Add((CanvasGroup)pipeGroup.Duplicate());
		pipeArray[1].SetProcess(false); // Stop the duplicate from processing 
		pipeArray[1].SetScript(dupeScript); // Attach the script to the duplicate
			

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

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta) {

		// If anything is pressed and nothing before it
		// then this means that the game has started
		if(Input.IsAnythingPressed() && !wasPressed){
			timer.Start(); // Start timer 
			// GD.Print(timer.WaitTime); // Print starting wait time
			wasPressed = true; // an input was pressed
			initPipe(pipeArray[0], true); // initate the first pipe
		}
	}

	
}
