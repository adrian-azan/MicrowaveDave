using Godot;
using Godot.Collections;
using System;
using System.Threading.Tasks;

public partial class Enemy : PathFollow3D
{
    public enum STATE
    {
        ATTACKING,
        IDLE
    };

    [Export]
    public AttackLanes.LANES _designatedLane;

    public STATE _state;

    protected AttackLanes _attackLanes;
    protected Array<Attack> _inFlightAttacks;

    protected int _health;
    public float _pace;

    protected Tween PROGRESS_TWEEN;
    protected Tween PACE_TWEEN;

    public override void _Ready()
    {
        _attackLanes = GetNode<AttackLanes>("../../../../../../AttackLanes");
        _inFlightAttacks = new Array<Attack>();
        ProgressRatio = Tools.rng.RandfRange(.50f, .65f);
        _pace = Tools.rng.RandfRange(.03f, .05f);
        _state = STATE.IDLE;

        _health = 1000;

        Bounce();
    }

    public override void _Process(double delta)
    {
        base._Process(delta);
    }

    public override void _ExitTree()
    {
        base._ExitTree();

        _attackLanes.ClearLanes(this);

        AudioManager._Instance.EnemyDeath();
    }

    public bool Dead()
    {
        return _health <= 0;
    }

    public void TakeDamage(int amount)
    {
        _health -= amount;
        AudioManager._Instance.EnemyDeath();
    }

    public void EnterBattle(Area3D area)
    {
        GD.Print("ATTACKING");
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