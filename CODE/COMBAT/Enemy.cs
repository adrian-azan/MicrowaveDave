using Godot;
using Godot.Collections;
using System;

public partial class Enemy : PathFollow3D
{
    public enum STATE
    {
        ATTACKING,
        IDLE
    };

    [Export]
    protected AttackLanes.LANES _designatedLane;

    public STATE _state;

    protected AttackLanes _attackLanes;
    protected Array<Attack> _inFlightAttacks;

    private int _health;
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

        Bounce();
    }

    public override void _Process(double delta)
    {
        base._Process(delta);
    }

    public override void _ExitTree()
    {
        base._ExitTree();

        _attackLanes.ClearLanes();
    }

    public bool Dead()
    {
        return _health <= 0;
    }

    public void EnterBattle(Area3D area)
    {
        GD.Print("ATTACKING");
        _state = STATE.ATTACKING;
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