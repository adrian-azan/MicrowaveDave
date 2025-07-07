using Godot;

public partial class SimpleStraight : Enemy
{
    public override void _Ready()
    {
        base._Ready();

        _health = 100;
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
        _attackLanes.SetLaneCurve(_designatedLane, ResourceLoader.Load<Curve2D>("res://SCENES/Battle System/Jab Types/DEFAULT.tres"));

        attackInstance.PROGRESS_TWEEN = CreateTween();
        attackInstance.PROGRESS_TWEEN.TweenProperty(attackInstance, "progress_ratio", .05, 1);
        attackInstance.PROGRESS_TWEEN.TweenProperty(attackInstance, "progress_ratio", 1, 3);
    }
}