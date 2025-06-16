using Godot;
using System;
using System.Threading.Tasks;

public partial class SimpleCrissCross : Enemy
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
            GetNode<Timer>("Timer").Autostart = false;
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
        _attackLanes.AddAttackToLane(attackInstance, AttackLanes.LANES.LEFT);
        _attackLanes.SetLaneCurve(AttackLanes.LANES.LEFT, ResourceLoader.Load<Curve2D>("res://SCENES/Battle System/Jab Types/SimpleCrissCross.tres"));

        attackInstance.PROGRESS_TWEEN = CreateTween();

        attackInstance.PROGRESS_TWEEN.SetTrans(Tween.TransitionType.Spring);
        attackInstance.PROGRESS_TWEEN.TweenProperty(attackInstance, "progress_ratio", .48, .5);

        attackInstance.PROGRESS_TWEEN.SetTrans(Tween.TransitionType.Linear);
        attackInstance.PROGRESS_TWEEN.TweenProperty(attackInstance, "progress_ratio", 1, .5).SetDelay(1);
    }
}