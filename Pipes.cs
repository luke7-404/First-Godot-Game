using Godot;
using System;
using System.Threading;

public partial class Pipes : Node2D
{
	private Area2D loadArea(){
		Sprite2D leadSprite = this.GetNode<Sprite2D>("Sprite2D");
		Area2D touchedPipe = leadSprite.GetNode<Area2D>("Area2D");
		//GD.Print(touchedPipe);
		return touchedPipe;
	}
	
	float randY;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		
		randY = GD.RandRange(-40,340) + GD.Randf();
		
		GD.Print(this.Position);
		this.Position = new Vector2(0, randY);
		GD.Print("Moved Position " + this.Position);
		
		loadArea().Monitoring = true;
		
	}
	
	

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		this.Position += new Vector2(-10, 0);
		
	}
}
