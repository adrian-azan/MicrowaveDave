using Godot;
using System;
using System.Threading.Tasks;

public partial class SimpleCrissCross : Enemy
{
    public override void _Ready()
    {
        _attackCoolDown = 4;
        base._Ready();
        _health = 30;
    }
    
    public override void _Process(double delta)
    {
        base._Process(delta);

        if (_state == STATE.ATTACKING && GetNode<Timer>("Timer").IsStopped())
        {
            AttackPlayer();
            GetNode<Timer>("Timer").Start();
        }
    }
    
    public new void AttackPlayer()
    { 
        Attack attackInstance = base.AttackPlayer();

        if (_designatedLane == Lanes.LANES.LEFT)
            _attackLanes.SetLaneCurve(_designatedLane, ResourceLoader.Load<Curve2D>("res://SCENES/Battle System/Jab Types/SimpleCrissCross_left.tres"));
        else if (_designatedLane == Lanes.LANES.RIGHT)
            _attackLanes.SetLaneCurve(_designatedLane, ResourceLoader.Load<Curve2D>("res://SCENES/Battle System/Jab Types/SimpleCrissCross_right.tres"));

        attackInstance.PROGRESS_TWEEN = CreateTween();

        attackInstance.PROGRESS_TWEEN.SetTrans(Tween.TransitionType.Spring);
        attackInstance.PROGRESS_TWEEN.TweenProperty(attackInstance, "progress_ratio", .48, .5);

        attackInstance.PROGRESS_TWEEN.SetTrans(Tween.TransitionType.Linear);
        attackInstance.PROGRESS_TWEEN.TweenProperty(attackInstance, "progress_ratio", 1, .5).SetDelay(1);
    }
}