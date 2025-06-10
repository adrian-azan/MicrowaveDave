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

    public STATE _state;

    protected AttackLanes _attackLanes;
    protected Array<Attack> _inFlightAttacks;

    private int _health;
    public float _pace;

    protected Tween INPROGRESS;

    public override void _Ready()
    {
        _attackLanes = GetNode<AttackLanes>("../../../../../../AttackLanes");
        _inFlightAttacks = new Array<Attack>();
        ProgressRatio = Tools.rng.RandfRange(.50f, .65f);
        _pace = .1f;
        _state = STATE.IDLE;
    }

    public override void _Process(double delta)
    {
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
}