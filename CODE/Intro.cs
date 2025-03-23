using Godot;
using System;

public partial class Intro : Node3D
{
    public int skip = 0;

    public override void _Process(double delta)
    {
        if (Input.IsActionJustPressed("Skip"))
        { skip++; }

        for (int i = 0; i < skip && i < 3; i++)
        {
            Tools.GetChildren<ColorRect>(this)[i].Color = new Color(1, 1, 1, .8f);
        }

        if (skip == 3)
            ExitIntro();
    }

    public void ExitIntro()
    {
        GetNode<AnimationPlayer>("AnimationPlayer").Stop();
        GetTree().ChangeSceneToFile("res://SCENES/MainScreen.tscn");
    }
}