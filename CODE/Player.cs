using Godot;
using System;

public partial class Player : Node3D
{
	private AnimationPlayer _animationPlayer;

	private float _bobbingSpeed;
	
	[Export]
	public float BobbingSpeed
	{
		get { return _bobbingSpeed;}
		set
		{
			_bobbingSpeed = value;
			if (_animationPlayer == null) return;
			_animationPlayer.SpeedScale = _bobbingSpeed;
		}
	}
	public override void _Ready()
	{
		_animationPlayer = (GetNode("AnimationPlayer") as AnimationPlayer);
		_animationPlayer.Play("bounce");
	}
}
