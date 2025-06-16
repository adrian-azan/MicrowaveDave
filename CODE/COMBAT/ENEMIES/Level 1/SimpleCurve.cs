using Godot;
using System;
using System.Threading.Tasks;

public partial class SimpleCurve : Enemy
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
        _attackLanes.SetLaneCurve(_designatedLane, ResourceLoader.Load<Curve2D>("res://SCENES/Battle System/Jab Types/SimpleCurve.tres"));

        attackInstance.PACE_TWEEN = CreateTween();
        attackInstance.PACE_TWEEN.TweenProperty(attackInstance, "_pace", .1, 1.1);
        attackInstance.PACE_TWEEN.TweenProperty(attackInstance, "_pace", 0, .2);
        attackInstance.PACE_TWEEN.TweenProperty(attackInstance, "_pace", -.05, .025);
        attackInstance.PACE_TWEEN.TweenProperty(attackInstance, "_pace", .7, .2).SetDelay(1);
    }
}