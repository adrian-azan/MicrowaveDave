using Godot;
using Godot.Collections;
using Godot.NativeInterop;
using System;
using System.Threading.Tasks;

public partial class SimpleStraight : Enemy
{
    public override void _Ready()
    {
        base._Ready();
    }

    public override void _Process(double delta)
    {
        base._Process(delta);

        if (_state == STATE.ATTACKING && GetNode<Timer>("Timer").IsStopped())
        {
            StartAttack();
            GetNode<Timer>("Timer").Start();
        }
    }

    public async void StartAttack()
    {
        await AttackPlayer();
    }

    public async Task AttackPlayer()
    {
        var attackScene = ResourceLoader.Load<PackedScene>(Constants.ATTACK_SCENE);

        var attackInstance = attackScene.Instantiate<Attack>();
        _attackLanes.AddAttackToLane(attackInstance, _designatedLane);
        _attackLanes.SetLaneCurve(AttackLanes.LANES.MIDDLE, ResourceLoader.Load<Curve2D>("res://SCENES/Battle System/Jab Types/DEFAULT.tres"));

        attackInstance.PROGRESS_TWEEN = CreateTween();
        attackInstance.PROGRESS_TWEEN.TweenProperty(attackInstance, "progress_ratio", 1, 3);
    }
}