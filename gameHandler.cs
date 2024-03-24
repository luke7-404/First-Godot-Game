using Godot;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

public partial class gameHandler : Node2D
{
	byte indx = 1;
	bool first = true;
	List<Node2D> createdPipeList = new List<Node2D>();
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Label scoreLable = GetNode<Control>("Control").GetNode<Label>("Label");
		scoreLable.Text = 0.ToString();

		Node2D pipeNode = GetNode<Node2D>("Pipes");
		Variant pipeScript = pipeNode.GetScript();
		createdPipeList.Add(pipeNode);

		
		for(byte i = 0; i < 2; i++){
			Node2D dupePipe = (Node2D)pipeNode.Duplicate();
			AddChild(dupePipe);
			dupePipe.SetScript(pipeScript);
			dupePipe.SetProcess(false);
			createdPipeList.Add(dupePipe);
			GD.Print("Pipe Added"); 
		}

		
		Timer timer = this.GetNode<Timer>("spawnClock");
		timer.WaitTime = 2f;
		timer.Start();
	}
	
	

	void onTimeout(){
		
		
		createdPipeList[indx].SetProcess(true);


		
	}

	public void onDelArea(Node2D area){

		//GetTree().Paused = true;
		indx++;
	}


	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	
}
