using Godot;
using System;

public partial class BattleManager : Node3D
{
    private DefenseLanes _defenseLanes;

    public override void _Ready()
    {
        // _defenseLanes = GetNode<DefenseLanes>(Constants.DEFENSE_LANES_FROM_PLAYER);
    }

    public override void _Process(double delta)
    {
        if (Input.IsActionPressed("LeftAttack"))
        {
            //      Attack nextAttack = ResourceLoader.Load<PackedScene>("res://SCENES/Battle System/Jab Types/Attack.tscn").Instantiate() as Attack;
            //      nextAttack.Set(Json.ParseString(FileAccess.GetFileAsString("res://CODE/COMBAT/ATTACKS/Player_LeftAttack.json")).AsGodotDictionary(), 0);

            //      _defenseLanes.AddAttackToLane(nextAttack);
        }

        if (Input.IsActionPressed("MiddleAttack"))
        {
        }

        if (Input.IsActionPressed("RightAttack"))
        {
        }
    }
}