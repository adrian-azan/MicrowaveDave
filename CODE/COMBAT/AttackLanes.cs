using Godot;
using Godot.Collections;

public partial class AttackLanes : Node2D
{
    private Array<Path2D> _lanes;

    public override void _Ready()
    {
        _lanes = Tools.GetChildren<Path2D>(this);
    }

    public override void _Process(double delta)
    {
        float felta = (float)delta;
    }

    public void AddAttackToLane(Attack attack, int lane)
    {
        GD.Print("Creating an Attack");
        _lanes[lane].AddChild(attack);
    }

    public int Count()
    {
        return _lanes == null ? 0 : _lanes.Count;
    }

    public void CollisionWithHeart(Area2D incomingAttack)
    {
        GD.Print(incomingAttack.GetParent());

        incomingAttack.GetParent().QueueFree();
    }
}