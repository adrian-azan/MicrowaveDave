using Godot;

using System.Linq;

public partial class AttackLanes : Lanes
{
    public override void _Ready()
    {
        _lanes = Tools.GetChildren<Path2D>(this);
    }

    public override void _Process(double delta)
    {
        float felta = (float)delta;
    }

    public new void ClearLanes()
    {
        _lanes.ToList().ForEach(lane =>
            lane.GetChildren()
            .ToList().ForEach(attack => attack.QueueFree())
            );
    }

    public void CollisionWithHeart(Area2D incomingAttack)
    {
        incomingAttack.GetParent().QueueFree();
    }

    public void AttackedByPlayer(Area2D incomingAttack)
    {
        incomingAttack.GetParent().QueueFree();

        GetNode("/root/CustomSignals").EmitSignal(CustomSignals.SignalName.SuccesfulAttackSignal, 25);
    }
}