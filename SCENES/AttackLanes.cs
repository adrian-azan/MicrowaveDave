using Godot;
using Godot.Collections;

public partial class AttackLanes : Node2D
{
    private Array<Path2D> _Lanes;

    [Export]
    private PackedScene _JabScene;

    public override void _Ready()
    {
        _Lanes = Tools.GetChildren<Path2D>(this);
        //  (GetNode("Timer") as Timer).Start();
    }

    public override void _Process(double delta)
    {
        float felta = (float)delta;
    }

    public void CreateJabs()
    {
        var newJab = _JabScene.Instantiate();
        newJab.GetNode<AnimationPlayer>("AnimationPlayer").Play("root");
        newJab.GetNode<Area2D>("Area2D").SetCollisionLayerValue(2, true);
        newJab.GetNode<Area2D>("Area2D").SetCollisionLayerValue(1, false);
        newJab.GetNode<Area2D>("Area2D").SetCollisionMaskValue(1, false);
        newJab.GetNode<Area2D>("Area2D").SetCollisionMaskValue(2, false);

        GetChild(0).AddChild(newJab);

        GetNode<Timer>("Timer").WaitTime = new RandomNumberGenerator().RandfRange(1, 3);
    }

    public void CollisionWithHeart(Area2D incomingAttack)
    {
        GD.Print(incomingAttack.GetParent());

        incomingAttack.GetParent().QueueFree();
    }
}