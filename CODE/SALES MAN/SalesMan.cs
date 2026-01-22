using Godot;
using System;

public partial class SalesMan : Node3D
{
	public enum Sales_State
	{
		TALKING,
		BUYING
	}

	public Sales_State state;
	
	public override void _Ready()
	{
		state = Sales_State.TALKING;
	}

	public override void _Process(double delta)
	{
	}
	
	
}
