using Godot;
using System;

public partial class LevelLoader : Node2D
{
	private SubViewport _viewport;
	public enum LEVEL
	{
		one,
		two
	}
	
	public override void _Ready()
	{
		_viewport = GetNode<SubViewport>("SubViewportContainer/SubViewport");

		CustomSignals._Instance.SwapLevel += SwapLevel;
	}

	private void SwapLevel(LEVEL level)
	{
		switch (level)
		{
			case LEVEL.one:
				var level1 = ResourceLoader.Load<PackedScene>("res://SCENES/LEVEL_1.tscn").Instantiate();
				_viewport.GetChild(0).QueueFree();
				_viewport.AddChild(level1);
			break;
			case LEVEL.two:
				var level2 = ResourceLoader.Load<PackedScene>("res://SCENES/LEVEL_1_2.tscn").Instantiate();
				_viewport.GetChild(0).QueueFree();
				_viewport.AddChild(level2);
				level2.GetNode<HallwayDisco>("HallwayDisco").Start();
			break;
		}
	}
	
}
