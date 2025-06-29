using Godot;
using Godot.Collections;
using System;
using System.Linq;

public partial class Hallway : Node3D
{
    [Export]
    private PackedScene piece;

    private Array<PathFollow3D> _pieces;

    [Export]
    private Array<PackedScene> _availableEnemies;

    public Array<Path3D> _lanes;

    public static float _pace;
    public static float _enemyPace;

    [Export]
    public int _DEBUG_enemyPaceMultiplyer = 1;

    public override void _Ready()
    {
        _pace = .1f;
        float totalPieces = 24;
        _pieces = new Array<PathFollow3D>();
        for (int i = 0; i < totalPieces; i++)
        {
            _pieces.Add(piece.Instantiate() as PathFollow3D);
            GetNode("Path3D").AddChild(_pieces[i]);
            _pieces[i].ProgressRatio = (float)i / totalPieces;
        }

        _lanes = new Array<Path3D>();
        _lanes.Add(GetNode<Path3D>("LeftLane"));
        _lanes.Add(GetNode<Path3D>("MiddleLane"));
        _lanes.Add(GetNode<Path3D>("RightLane"));

        var timeline = GetNode<AnimationPlayer>("AnimationPlayer");
        timeline?.Play("LEVEL_ONE");

        GetNode<CustomSignals>("/root/CustomSignals").SuccesfulAttackSignal += DamageEnemy;
    }

    public override void _Process(double delta)
    {
        float felta = (float)delta;
        foreach (PathFollow3D piece in _pieces)
        {
            piece.ProgressRatio += felta * _pace;
        }

        foreach (Enemy enemy in GetTree().GetNodesInGroup("Enemy"))
        {
            if (enemy.ProgressRatio > .07 && enemy.ProgressRatio < .1)
                continue;

            enemy.ProgressRatio += felta * enemy._pace * _DEBUG_enemyPaceMultiplyer;
        }

        GetNode<Label>("Label").Text = Math.Round(GetNode<AnimationPlayer>("AnimationPlayer").CurrentAnimationPosition).ToString();
    }

    /*
     * Enemies are attacked in order of which lane they are in. Middle first, then left, then right.
     */

    public void DamageEnemy(int amount)
    {
        Array<Enemy> enemiesInLanes = new Array<Enemy>();
        enemiesInLanes.Add(_lanes[0].GetChildCount() > 0 ? _lanes[0].GetChild<Enemy>(0) : null);
        enemiesInLanes.Add(_lanes[1].GetChildCount() > 0 ? _lanes[1].GetChild<Enemy>(0) : null);
        enemiesInLanes.Add(_lanes[2].GetChildCount() > 0 ? _lanes[2].GetChild<Enemy>(0) : null);

        enemiesInLanes = new Array<Enemy>(enemiesInLanes.Where(enemy => enemy != null && enemy.ProgressRatio > .05f && enemy.ProgressRatio < .12));

        if (enemiesInLanes.Count == 0)
            return;

        enemiesInLanes[0].TakeDamage(amount);

        if (enemiesInLanes[0].Dead())
        {
            enemiesInLanes[0].QueueFree();
        }
    }

    public void TutorialPhaseOne()
    {
        _lanes[1].AddChild(_availableEnemies[0].Instantiate<Enemy>());

        GetNode<AnimationPlayer>("AnimationPlayer").Pause();
    }

    public void TutorialPhaseTwo()
    {
        var curve = _availableEnemies[1].Instantiate<Enemy>();

        curve._designatedLane = Lanes.LANES.MIDDLE;

        _lanes[1].AddChild(curve);

        GetNode<AnimationPlayer>("AnimationPlayer").Pause();
    }

    public void TutorialPhaseFour()
    {
        var normal = _availableEnemies[0].Instantiate<Enemy>();
        var curve = _availableEnemies[1].Instantiate<Enemy>();

        normal._designatedLane = Lanes.LANES.MIDDLE;
        curve._designatedLane = Lanes.LANES.LEFT;

        _lanes[1].AddChild(normal);
        _lanes[0].AddChild(curve);

        GetNode<AnimationPlayer>("AnimationPlayer").Pause();
    }

    public void PhaseOne()
    {
        var simpleStraight = _availableEnemies[0].Instantiate<Enemy>();
        simpleStraight._designatedLane = Lanes.LANES.MIDDLE;

        _lanes[1].AddChild(simpleStraight);
    }

    public void PhaseTwo()
    {
        var simpleStraight = _availableEnemies[0].Instantiate<Enemy>();
        var simpleStraight_2 = _availableEnemies[0].Instantiate<Enemy>();
        simpleStraight._designatedLane = Lanes.LANES.MIDDLE;
        simpleStraight_2._designatedLane = Lanes.LANES.LEFT;

        _lanes[1].AddChild(simpleStraight);

        CreateTween().TweenCallback(Callable.From(() => _lanes[0].AddChild(simpleStraight_2))).SetDelay(10);
    }

    public void PhaseThree()
    {
        var simpleCurve = _availableEnemies[1].Instantiate<Enemy>();
        var simpleStraight_2 = _availableEnemies[0].Instantiate<Enemy>();

        simpleCurve._designatedLane = Lanes.LANES.MIDDLE;
        simpleStraight_2._designatedLane = Lanes.LANES.LEFT;

        _lanes[1].AddChild(simpleCurve);
        _lanes[0].AddChild(simpleStraight_2);
    }

    public void PhaseFour()
    {
        var simpleStraight = _availableEnemies[0].Instantiate<Enemy>();
        var simpleCurve = _availableEnemies[1].Instantiate<Enemy>();
        var simpleStraight_2 = _availableEnemies[0].Instantiate<Enemy>();

        simpleStraight._designatedLane = Lanes.LANES.MIDDLE;
        simpleCurve._designatedLane = Lanes.LANES.LEFT;
        simpleStraight_2._designatedLane = Lanes.LANES.RIGHT;

        _lanes[0].AddChild(simpleStraight);
        _lanes[1].AddChild(simpleCurve);
        _lanes[2].AddChild(simpleStraight_2);
    }
}