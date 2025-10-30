using Godot;
using Godot.Collections;
using System;
using System.Linq;
using System.Linq.Expressions;

public partial class Hallway : Node3D
{
    [Export]
    private PackedScene piece;

    private Array<PathFollow3D> _pieces;
    
    public Array<Path3D> _lanes;

    [Export]
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
    }

    public override void _Process(double delta)
    {
        float felta = (float)delta;
        foreach (PathFollow3D piece in _pieces)
        {
            piece.ProgressRatio += felta * _pace;
        }

        Array<Array<Enemy>> Lanes = new Array<Array<Enemy>>();
        Lanes.Add(Tools.GetChildren<Enemy>(_lanes[1]));
        Lanes.Add(Tools.GetChildren<Enemy>(_lanes[0]));
        Lanes.Add(Tools.GetChildren<Enemy>(_lanes[2]));

        foreach (Array<Enemy> lane in Lanes)
        {
            if (lane.Count == 0 || (lane.First().ProgressRatio > .9 && lane.First().ProgressRatio < .95))
                continue;

            lane.First().ProgressRatio += felta * lane.First()._pace * _DEBUG_enemyPaceMultiplyer;
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
}