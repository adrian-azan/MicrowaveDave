using Godot;

public partial class SimpleCurve : Enemy
{
    public override void _Ready()
    {
        base._Ready();

        _health = 100;
    }
    
    public new void AttackPlayer()
    {
        Attack attackInstance = base.AttackPlayer();

        _attackLanes.SetLaneCurve(_designatedLane, ResourceLoader.Load<Curve2D>("res://SCENES/Battle System/Jab Types/SimpleCurve.tres"));

        attackInstance.PACE_TWEEN = CreateTween();
        attackInstance.PACE_TWEEN.TweenProperty(attackInstance, "_pace", .1, 1.1);
        attackInstance.PACE_TWEEN.TweenProperty(attackInstance, "_pace", 0, .2);
        attackInstance.PACE_TWEEN.TweenProperty(attackInstance, "_pace", -.05, .025);
        attackInstance.PACE_TWEEN.TweenProperty(attackInstance, "_pace", .7, .2).SetDelay(1);
    }
}