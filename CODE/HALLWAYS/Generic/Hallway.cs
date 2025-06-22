using Godot;
using Godot.Collections;
using System;
using System.Linq;

public partial class Hallway : Node3D
{
    [Export]
    public PackedScene piece;

    [Export]
    public Array<PackedScene> _availableEnemies;

    public Array<PathFollow3D> pieces;

    public static float _pace;
    public static float _enemyPace;

    [Export]
    public int _DEBUG_enemyPaceMultiplyer = 1;

    public override void _Ready()
    {
        _pace = .1f;
        float totalPieces = 24;
        pieces = new Array<PathFollow3D>();
        for (int i = 0; i < totalPieces; i++)
        {
            pieces.Add(piece.Instantiate() as PathFollow3D);
            GetNode("Path3D").AddChild(pieces[i]);
            pieces[i].ProgressRatio = (float)i / totalPieces;
        }

        var timeline = GetNode<AnimationPlayer>("AnimationPlayer");
        timeline?.Play("LEVEL_ONE");

        GetNode<CustomSignals>("/root/CustomSignals").SuccesfulAttackSignal += DamageEnemy;
    }

    public override void _Process(double delta)
    {
        float felta = (float)delta;
        foreach (PathFollow3D piece in pieces)
        {
            piece.ProgressRatio += felta * _pace;
        }

        foreach (Enemy enemy in GetTree().GetNodesInGroup("Enemy"))
        {
            if (enemy.ProgressRatio > .07 && enemy.ProgressRatio < .1)
                continue;

            enemy.ProgressRatio += felta * enemy._pace * _DEBUG_enemyPaceMultiplyer;
        }
    }

    /*
     * Enemies are attacked in order of which lane they are in. Middle first, then left, then right.
     */

    public void DamageEnemy(int amount)
    {
        Array<Enemy> enemiesInLanes = new Array<Enemy>();
        enemiesInLanes.Add(GetNode("MiddleLane").GetChildCount() > 0 ? GetNode("MiddleLane").GetChild<Enemy>(0) : null);
        enemiesInLanes.Add(GetNode("LeftLane").GetChildCount() > 0 ? GetNode("LeftLane").GetChild<Enemy>(0) : null);
        enemiesInLanes.Add(GetNode("RightLane").GetChildCount() > 0 ? GetNode("RightLane").GetChild<Enemy>(0) : null);

        enemiesInLanes = new Array<Enemy>(enemiesInLanes.Where(enemy => enemy != null && enemy.ProgressRatio > .05f && enemy.ProgressRatio < .12));

        if (enemiesInLanes.Count == 0)
            return;

        enemiesInLanes[0].TakeDamage(amount);

        if (enemiesInLanes[0].Dead())
        {
            enemiesInLanes[0].QueueFree();
        }
    }

    public void TutorialNextStep()
    {
        var timeline = GetNode<AnimationPlayer>("AnimationPlayer");
        timeline?.Play("TIME_LINE");
    }

    public void TutorialPhaseOne()
    {
        GetNode("MiddleLane").AddChild(_availableEnemies[0].Instantiate<Enemy>());

        GetNode<AnimationPlayer>("AnimationPlayer").Pause();
    }

    public void TutorialPhaseTwo()
    {
        var curve = _availableEnemies[1].Instantiate<Enemy>();

        curve._designatedLane = AttackLanes.LANES.MIDDLE;

        GetNode("MiddleLane").AddChild(curve);

        GetNode<AnimationPlayer>("AnimationPlayer").Pause();
    }

    public void TutorialPhaseFour()
    {
        var normal = _availableEnemies[0].Instantiate<Enemy>();
        var curve = _availableEnemies[1].Instantiate<Enemy>();

        normal._designatedLane = AttackLanes.LANES.MIDDLE;
        curve._designatedLane = AttackLanes.LANES.LEFT;

        GetNode("MiddleLane").AddChild(normal);
        GetNode("LeftLane").AddChild(curve);

        GetNode<AnimationPlayer>("AnimationPlayer").Pause();
    }

    public void PhaseOne()
    {
        var normal = _availableEnemies[0].Instantiate<Enemy>();
        var curve = _availableEnemies[0].Instantiate<Enemy>();

        normal._designatedLane = AttackLanes.LANES.MIDDLE;
        curve._designatedLane = AttackLanes.LANES.LEFT;

        GetNode("MiddleLane").AddChild(normal);
        GetNode("LeftLane").AddChild(curve);
    }
}