using Godot;
using System;

public partial class GameOver : Node2D
{
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		GetNode<Button>("Retry").GrabFocus();
		CustomSignals._Instance.EmitSignal(CustomSignals.SignalName.Stop);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (Input.IsActionJustPressed("Skip") && GetViewport().GuiGetFocusOwner() == GetNode("Quit"))
		{
			GetTree().Quit();
		}
		if (Input.IsActionJustPressed("Skip") && GetViewport().GuiGetFocusOwner() == GetNode("Retry"))
		{
			CustomSignals._Instance.EmitSignal(CustomSignals.SignalName.Reset);
			QueueFree();
		}
		
		
	}
}
