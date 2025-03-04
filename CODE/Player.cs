using Godot;
using System;

public partial class Player : Node3D
{
    public override void _Ready()
    {
        (GetNode("AnimationPlayer") as AnimationPlayer).Play("bounce");
    }
}