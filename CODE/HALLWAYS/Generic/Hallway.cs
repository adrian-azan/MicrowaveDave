using Godot;
using Godot.Collections;
using System;
using System.Collections;
using System.Linq;
using System.Linq.Expressions;

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

        CustomSignals._Instance.SuccesfulAttackSignal += DamageEnemy;

        GetNode<AnimationPlayer>("AnimationPlayer").Play("LEVEL_ONE");
    }

    public override void _Process(double delta)
    {
        float felta = (float)delta;
        foreach (PathFollow3D piece in _pieces)
        {
            piece.ProgressRatio += felta * _pace;
        }

        Array<Enemy> MiddleLaneEnemies = Tools.GetChildren<Enemy>(_lanes[1]);
        Array<Enemy> LeftLaneEnemies = Tools.GetChildren<Enemy>(_lanes[0]);
        Array<Enemy> RightLaneEnemies = Tools.GetChildren<Enemy>(_lanes[2]);

        /* foreach (Enemy enemy in MiddleLaneEnemies)
         {
             if (enemy.ProgressRatio > .07 && enemy.ProgressRatio < .1)
                 continue;

             enemy.ProgressRatio += felta * enemy._pace * _DEBUG_enemyPaceMultiplyer;
         }*/

        for (int i = 0; i < MiddleLaneEnemies.Count; i++)
        {
            if (MiddleLaneEnemies[i].ProgressRatio > .9 - (i * .1) && MiddleLaneEnemies[i].ProgressRatio < .95 - (i * .1))
                continue;

            MiddleLaneEnemies[i].ProgressRatio += felta * MiddleLaneEnemies[i]._pace * _DEBUG_enemyPaceMultiplyer;
        }
    }

    /*
     * Enemies are attacked in order of which lane they are in. Middle first, then left, then right.
     */

    public void DamageEnemy(int amount)
    {
        Array<Enemy> enemiesInLanes = new Array<Enemy>();
        enemiesInLanes.Add(_lanes[1].GetChildCount() > 0 ? _lanes[1].GetChild<Enemy>(0) : null);
        enemiesInLanes.Add(_lanes[0].GetChildCount() > 0 ? _lanes[0].GetChild<Enemy>(0) : null);
        enemiesInLanes.Add(_lanes[2].GetChildCount() > 0 ? _lanes[2].GetChild<Enemy>(0) : null);

        enemiesInLanes = new Array<Enemy>(enemiesInLanes.Where(enemy => enemy != null));

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

        var simpleStraight2 = _availableEnemies[0].Instantiate<Enemy>();
        simpleStraight2._designatedLane = Lanes.LANES.MIDDLE;

        _lanes[1].AddChild(simpleStraight2);
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

        _lanes[0].AddChild(simpleCurve);
        _lanes[1].AddChild(simpleStraight);
        _lanes[2].AddChild(simpleStraight_2);
    }
}