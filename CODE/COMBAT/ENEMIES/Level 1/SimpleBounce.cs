using Godot;
using System;
using System.Threading.Tasks;

public partial class SimpleBounce : Enemy
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
        attackInstance.Attacker = this;

        _attackLanes.AddAttackToLane(attackInstance, AttackLanes.LANES.MIDDLE);
        _attackLanes.SetLaneCurve(AttackLanes.LANES.MIDDLE, ResourceLoader.Load<Curve2D>("res://SCENES/Battle System/Jab Types/DEFAULT.tres"));

        attackInstance.PACE_TWEEN = CreateTween();

        attackInstance.PACE_TWEEN.TweenProperty(attackInstance, "_pace", .5, .02);
        attackInstance.PACE_TWEEN.TweenProperty(attackInstance, "_pace", -2, .3).SetDelay(1.5f);
        attackInstance.PACE_TWEEN.TweenProperty(attackInstance, "_pace", 2, .3).SetDelay(.1f);
        attackInstance.PACE_TWEEN.TweenProperty(attackInstance, "_pace", -2, .3).SetDelay(.1f);
        attackInstance.PACE_TWEEN.TweenProperty(attackInstance, "_pace", 2, .3).SetDelay(.1f);
        attackInstance.PACE_TWEEN.TweenProperty(attackInstance, "_pace", -2, .3).SetDelay(.1f);
        attackInstance.PACE_TWEEN.TweenProperty(attackInstance, "_pace", 2, .3).SetDelay(.1f);
    }
}