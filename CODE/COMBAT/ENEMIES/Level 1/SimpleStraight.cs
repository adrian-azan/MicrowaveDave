using Godot;

public partial class SimpleStraight : Enemy
{
    //TODO: Get Timer out of scene tree and just instantiate in enemy classes
    //so that I can controll the refresh rate easier
    public override void _Ready()
    {
        base._Ready();

        _health = 10;
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
        Logging.PrintTemp("HEY");
        attackInstance.PROGRESS_TWEEN = CreateTween();
        attackInstance.PROGRESS_TWEEN.TweenProperty(attackInstance, "progress_ratio", .05, 1);
        attackInstance.PROGRESS_TWEEN.TweenProperty(attackInstance, "progress_ratio", 1, 3);
    }

    
    //TODO: Maybe play around with when you hit an enemy, theyre own attack timer resets
    //Some enemies you can easily get in a loop, others the reset does less and less, and maybe some your building up
    //an unslaught attack
    public new void TakeDamage(int amount)
    {
        base.TakeDamage(amount);
        
        
    }

}