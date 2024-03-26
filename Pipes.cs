using Godot;
using System;

public partial class Pipes : CanvasGroup
{
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{	
	}
	
	
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta){	
		
		// Move pipe
		Position += new Vector2(-7.5f, 0);
		
	}

	
}
