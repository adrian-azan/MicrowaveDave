using Godot;
using System;

public partial class Player : Node3D
{
    private AnimationPlayer _animationPlayer;

    private float _speed;
    
    [Export]
    public float Speed
    {
        get { return _speed;}
        set
        {
            _speed = value;
            if (_animationPlayer == null) return;
            _animationPlayer.SpeedScale = _speed;
        }
    }
    public override void _Ready()
    {
        _animationPlayer = (GetNode("AnimationPlayer") as AnimationPlayer);
        _animationPlayer.Play("bounce");
    }
}