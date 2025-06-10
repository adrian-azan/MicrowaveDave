using Godot;
using Godot.Collections;

public partial class Hallway : Node3D
{
    [Export]
    public PackedScene piece;

    public Array<PathFollow3D> pieces;

    public static float _pace;
    public static float _enemyPace;

    [Export]
    public PackedScene LEFT_ENEMY;

    [Export]
    public PackedScene MIDDLE_ENEMY;

    [Export]
    public PackedScene RIGHT_ENEMY;

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
        timeline?.Play("TIME_LINE");
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

            enemy.ProgressRatio += felta * enemy._pace;
        }
    }

    public void TutorialNextStep()
    {
        var timeline = GetNode<AnimationPlayer>("AnimationPlayer");
        timeline?.Play("TIME_LINE");
    }

    public void PhaseOne()
    {
        GetNode("LeftLane").AddChild(MIDDLE_ENEMY.Instantiate<Enemy>());
        GetNode("RightLane").AddChild(RIGHT_ENEMY.Instantiate<Enemy>());
        GetNode("MiddleLane").AddChild(LEFT_ENEMY.Instantiate<Enemy>());

        GetNode<AnimationPlayer>("AnimationPlayer").Pause();
    }

    public void PhaseTwo()
    {
        GetNode("LeftLane").AddChild(LEFT_ENEMY.Instantiate<Enemy>());
    }
}