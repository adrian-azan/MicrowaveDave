using Godot;
using System;

public partial class Intro : Node3D
{
    public int skip = 0;
    public override void _Process(double delta)
    {
        if (Input.IsActionJustPressed("Skip"))
        { skip++; }

        if (skip == 3)
            ExitIntro();
    }
    public void ExitIntro()
    {
        GetNode<AnimationPlayer>("AnimationPlayer").Stop();
        GetTree().ChangeSceneToFile("res://SCENES/MainScreen.tscn");
    }
}