using Godot;
using System;
using System.Threading;

public partial class Pipes : CanvasGroup
{
	private float randY;
	public bool move;

	public void initPipe(bool value){
		randY = GD.RandRange(-40,340) + GD.Randf();
		
		GD.Print(this.Position);
		this.Position = new Vector2(864, randY);
		GD.Print("Moved Position " + this.Position);
		
		this.GetNode<Area2D>("Area2D").Monitoring = true;
		move = value;
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		initPipe(false);
	}
	
	
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{	
		if (Input.IsAnythingPressed() && !move) move = true;
		
		if(move){
			this.Position += new Vector2(-7.5f, 0);
		} else {
			this.Position = new Vector2(864, 0);
		}
		
	}

	
}
