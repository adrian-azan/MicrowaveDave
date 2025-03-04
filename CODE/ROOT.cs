using Godot;
using System;

public partial class ROOT : Node3D
{
    public override void _Process(double delta)
    {
        if (Input.IsActionJustPressed("SwitchCamera"))
        {
            if (GetNode<Camera3D>("Camera3D").Current)
            {
                GetNode<Camera3D>("Camera3D2").Current = true;
                GetNode<WorldEnvironment>("WorldEnvironment").Environment.BackgroundColor = Color.Color8(255, 255, 255);
            }
            else
            {
                GetNode<Camera3D>("Camera3D").Current = true;
                GetNode<WorldEnvironment>("WorldEnvironment").Environment.BackgroundColor = Color.Color8(0, 0, 0);
            }
        }

        if (Input.IsActionPressed("ShowArt"))
            GetNode<Node2D>("../../../ConceptArt").Visible = true;
        else
            GetNode<Node2D>("../../../ConceptArt").Visible = false;
    }
}