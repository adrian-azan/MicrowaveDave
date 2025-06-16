using Godot;
using Godot.Collections;
using Godot.NativeInterop;
using System.Collections.Generic;
using System.Linq;

public partial class AttackLanes : Node2D
{
    public enum LANES
    {
        LEFT = 0,
        MIDDLE = 1,
        RIGHT = 2
    }

    private Array<Path2D> _lanes;

    public override void _Ready()
    {
        _lanes = Tools.GetChildren<Path2D>(this);
    }

    public override void _Process(double delta)
    {
        float felta = (float)delta;
    }

    public Array<Node> EnemiesInLane(LANES lane)
    {
        return _lanes[((int)lane)].GetChildren();
    }

    public void AddAttackToLane(Attack attack, LANES lane)
    {
        _lanes[(int)lane].AddChild(attack);
    }

    public void ClearLanes()
    {
        _lanes.ToList().ForEach(lane => lane.GetChildren().ToList()
        .ForEach(attack => attack.QueueFree()));
    }

    public void SetLaneCurve(LANES lane, Curve2D curve)
    {
        _lanes[(int)lane].Curve = curve;
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