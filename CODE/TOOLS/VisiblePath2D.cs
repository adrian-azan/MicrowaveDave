using Godot;
using System;
using Godot.Collections;

public partial class VisiblePath2D : Path2D
{
	[Export] public Texture2D icon;

	private Array<PathFollow2D> _followingIcons;
	
	public override void _Ready()
	{
		PathFollow2D iconGroup = new PathFollow2D();
		AddChild(iconGroup);

		_followingIcons = [];
		for (float i = 0; i < 100; i++)
		{
			PathFollow2D follow = new PathFollow2D();
			Sprite2D sprite = new Sprite2D();
			sprite.Scale *= .25f;
			sprite.Texture = icon;
			follow.AddChild(sprite);
			AddChild(follow);
			_followingIcons.Add(follow);
		}
		
	}
	public override void _Process(double delta)
	{
		float i = 0;
		foreach (PathFollow2D follow in _followingIcons)
		{
			if (Curve != null)
				follow.ProgressRatio = i++ / 100f;
		}
	}
}
