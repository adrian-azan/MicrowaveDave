using Godot;
using Godot.Collections;

public partial class DefenseLanes : Node2D
{
    private Array<Path2D> _Lanes;

    [Export]
    private PackedScene _JabScene;

    public override void _Ready()
    {
        _Lanes = Tools.GetChildren<Path2D>(this);
        (GetNode("Timer") as Timer).Start();
    }

    public override void _Process(double delta)
    {
        float felta = (float)delta;

        if (Input.IsActionJustPressed("LeftAttack"))
        {
            PathFollow2D defenseJab = _JabScene.Instantiate<PathFollow2D>();
            defenseJab.GetNode<AnimationPlayer>("AnimationPlayer").Play("root");

            _Lanes[0].AddChild(defenseJab);
        }
    }

    public void CollisionWithHeart(Area2D incomingAttack)
    {
        incomingAttack.GetParent().QueueFree();
    }
}