using Godot;
using Godot.Collections;
using System.Threading.Tasks;

public partial class Enemy : PathFollow3D
{
    public enum STATE
    {
        ATTACKING,
        IDLE
    };

    [Export]
    public Lanes.LANES _designatedLane;

    public STATE _state;

    protected AttackLanes _attackLanes;

    protected int _health;
    public float _pace;
    [Export]
    private float _attackCoolDown;

    protected Tween PROGRESS_TWEEN;
    protected Tween PACE_TWEEN;

    public Callable EndTask;

    public override void _Ready()
    {
        _attackLanes = GetTree().GetFirstNodeInGroup("AttackLane") as AttackLanes;
        ProgressRatio = Tools.rng.RandfRange(.30f, .45f);
        _pace = Tools.rng.RandfRange(.03f, .05f);
        _state = STATE.IDLE;

        _health = 1000;

        Bounce();
        GetNode<Timer>("Timer").SetWaitTime(_attackCoolDown);
    }

    public override void _ExitTree()
    {
        base._ExitTree();
        _attackLanes.ClearLanes(this);

        Logging.PrintTemp("HEY","HEY");
        CustomSignals._Instance.EmitSignal(CustomSignals.SignalName.EnemyKilled);
        
        AudioManager._Instance.EnemyDeath();

        if(EndTask.Delegate != null)
            EndTask.Call();
    }

    public bool Dead()
    {
        return _health <= 0;
    }

    public void TakeDamage(int amount)
    {
        _health -= amount;
        AudioManager._Instance.EnemyDeath();
        AudioManager._Instance.Punch();

        //Get Hit animation
        var side = new Array() { -1, 1 };

        float sideAngle = Tools.rng.RandfRange(-50, -40);
        sideAngle = sideAngle * side.PickRandom().AsInt32();

        var tween = CreateTween();
        tween.SetTrans(Tween.TransitionType.Bounce);
        tween.TweenProperty(this, "rotation", new Vector3(Mathf.DegToRad(-50), Mathf.DegToRad(sideAngle), 0), .25);
        tween.TweenProperty(this, "rotation", new Vector3(0, 0, 0), .5);
    }

    public void EnterBattle(Area3D area)
    {
        _state = STATE.ATTACKING;
    }

    public Attack AttackPlayer()
    {
        var attackScene = ResourceLoader.Load<PackedScene>(Constants.ATTACK_SCENE);
        var attackInstance = attackScene.Instantiate<Attack>();

        attackInstance.Attacker = this;
        _attackLanes.AddAttackToLane(attackInstance, _designatedLane);

        return attackInstance;
    }

    public void Bounce()
    {
        var tween = CreateTween();
        tween.SetTrans(Tween.TransitionType.Bounce);
        tween.TweenProperty(GetNode<MeshInstance3D>("MeshInstance3D"), "position", new Vector3(0, .3f, 0), .25f).AsRelative();
        tween.TweenProperty(GetNode<MeshInstance3D>("MeshInstance3D"), "position", new Vector3(0, -.3f, 0), .25f).AsRelative();

        tween.TweenCallback(Callable.From(() => Bounce()));
    }
}