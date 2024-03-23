using Godot;
using System;

public partial class freeArea : Area2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void onPipeLeave(Area2D area){
		GetParent<Node2D>().Free();
		GD.Print("Pipe Deleted");
	}
}
