using Godot;
using System;
using System.Threading.Tasks;

public partial class SimpleBounce : Enemy
{

    public new void AttackPlayer()
    {
        Attack attackInstance = base.AttackPlayer();

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