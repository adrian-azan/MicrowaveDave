using Godot;
using Godot.Collections;
using Godot.NativeInterop;
using System;
using System.Threading.Tasks;

public partial class Level1_Basic : Enemy
{
    [Export]
    private int _designatedLane;

    public override void _Ready()
    {
        base._Ready();

        _pace = Tools.rng.RandfRange(.03f, .05f);

        Bounce();
    }

    public void Bounce()
    {
        var tween = CreateTween();
        tween.SetTrans(Tween.TransitionType.Bounce);
        tween.TweenProperty(GetNode<MeshInstance3D>("MeshInstance3D"), "position", new Vector3(0, .3f, 0), .25f).AsRelative();
        tween.TweenProperty(GetNode<MeshInstance3D>("MeshInstance3D"), "position", new Vector3(0, -.3f, 0), .25f).AsRelative();

        tween.TweenCallback(Callable.From(() => Bounce()));
    }

    public override void _Process(double delta)
    {
        base._Process(delta);

        if (_state == STATE.ATTACKING && GetNode<Timer>("Timer").IsStopped())
        {
            StartAttack();
            GetNode<Timer>("Timer").Start();
        }
    }

    public async void StartAttack()
    {
        await AttackPlayer();
    }

    public async Task AttackPlayer()
    {
        var attack = ResourceLoader.Load<PackedScene>(Constants.ATTACK_SCENE);
        try
        {
            SimpleStraight();
        }
        catch (Exception e)
        {
            GD.Print(e);
        }
    }

    public void SimpleStraight()
    {
        var attack = ResourceLoader.Load<PackedScene>(Constants.ATTACK_SCENE);

        var test = attack.Instantiate<Attack>();
        _attackLanes.AddAttackToLane(test, _designatedLane);

        test.IN_PROGRESS = CreateTween();
        test.IN_PROGRESS.TweenProperty(test, "progress_ratio", 1, 3);

        test.IN_PROGRESS.TweenCallback(Callable.From(() => test.QueueFree()));
    }

    public void YeOldSwitchaRoo()
    {
        var attack = ResourceLoader.Load<PackedScene>(Constants.ATTACK_SCENE);

        var test = attack.Instantiate<Attack>();
        _attackLanes.AddAttackToLane(test, _designatedLane);
        test.IN_PROGRESS = CreateTween();
        test.IN_PROGRESS.SetTrans(Tween.TransitionType.Spring);
        test.IN_PROGRESS.TweenProperty(test, "progress_ratio", .75, 1);
        test.IN_PROGRESS.TweenProperty(test, "progress_ratio", .55, .5);

        test.IN_PROGRESS.SetTrans(Tween.TransitionType.Linear);
        test.IN_PROGRESS.TweenProperty(test, "progress_ratio", 1, .25).SetDelay(.5f);

        test.IN_PROGRESS.TweenCallback(Callable.From(() => test.QueueFree()));
    }
}