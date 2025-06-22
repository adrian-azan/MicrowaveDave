using Godot;
using Godot.Collections;
using System.Collections;
using System.Security.Cryptography;

public partial class DefenseLanes : Lanes
{
    public override void _Process(double delta)
    {
        float felta = (float)delta;

        if (Input.IsActionJustPressed("LeftAttack"))
        {
            var attackScene = ResourceLoader.Load<PackedScene>(Constants.ATTACK_SCENE);
            var attackInstance = attackScene.Instantiate<Attack>();
            attackInstance._pace = .4f;
            attackInstance.GetNode<Area2D>("Area2D").SetCollisionLayerValue(6, true);
            attackInstance.GetNode<Area2D>("Area2D").SetCollisionLayerValue(2, false);

            AddAttackToLane(attackInstance, LANES.LEFT);
        }

        if (Input.IsActionJustPressed("MiddleAttack"))
        {
            var attackScene = ResourceLoader.Load<PackedScene>(Constants.ATTACK_SCENE);
            var attackInstance = attackScene.Instantiate<Attack>();
            attackInstance._pace = .4f;
            attackInstance.GetNode<Area2D>("Area2D").SetCollisionLayerValue(6, true);
            attackInstance.GetNode<Area2D>("Area2D").SetCollisionLayerValue(2, false);

            AddAttackToLane(attackInstance, LANES.MIDDLE);
        }

        if (Input.IsActionJustPressed("RightAttack"))
        {
            var attackScene = ResourceLoader.Load<PackedScene>(Constants.ATTACK_SCENE);
            var attackInstance = attackScene.Instantiate<Attack>();
            attackInstance._pace = .4f;
            attackInstance.GetNode<Area2D>("Area2D").SetCollisionLayerValue(6, true);
            attackInstance.GetNode<Area2D>("Area2D").SetCollisionLayerValue(2, false);

            AddAttackToLane(attackInstance, LANES.RIGHT);
        }
    }

    public void CollisionWithHeart(Area2D incomingAttack)
    {
        incomingAttack.GetParent().QueueFree();
    }
}