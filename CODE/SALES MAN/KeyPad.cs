using Godot;
using System;

public partial class KeyPad : Node3D
{
	private Node3D Hand;
	
	public override void _Process(double delta)
	{
		Hand = GetNode<Node3D>("Hand");
	}

	public void HandleInput()
	{
		Vector3 currentPosition = Hand.Position;
		if (Input.IsActionPressed("MoveHandLeft"))
		{
			currentPosition.Z += 0.1f;
		}
		
		else if (Input.IsActionPressed("MoveHandRight"))
		{
			currentPosition.Z -= 0.1f;
		}
	}
}
