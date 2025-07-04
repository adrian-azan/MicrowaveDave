using Godot;
using System;
using Godot.Collections;
using System.Linq;

public partial class EnemyTracker : VBoxContainer
{
    public int _enemyTracker;
    public Dictionary<string, Array<Control>> _options;

    [Export]
    private PackedScene[] enemies;

    public override void _Ready()
    {
        _options = new Dictionary<string, Array<Control>>();
        Right();
    }

    public override void _Process(double delta)
    {
        Control currentFocus = GetNode<Label>("EnemyType");
        var lane = GetNode("/root/MainScreen/SubViewportContainer/SubViewport/Root/Hallway/MiddleLane");

        GetNode<Label>("EnemyType").Text = System.String.Format("EnemyType: {0}", lane.GetChildren()[0].Name);
        GetNode<Sprite2D>("Sprite2D2").Position = new Vector2(GetNode<Sprite2D>("Sprite2D2").Position.X,
                                                                currentFocus.Position.Y + currentFocus.Size.Y * .5f);

        if (Input.IsActionJustPressed("Increase"))
        {
            Right();
        }

        if (Input.IsActionJustPressed("Decrease"))
        {
            Left();
        }
    }

    public void Right()
    {
        _enemyTracker += 1;
        _enemyTracker = _enemyTracker >= enemies.Count() ? 0 : _enemyTracker;

        CreateEnemy();
    }

    public void Left()
    {
        _enemyTracker -= 1;
        _enemyTracker = _enemyTracker < 0 ? enemies.Count() - 1 : _enemyTracker;

        CreateEnemy();
    }

    public void CreateEnemy()
    {
        var lane = GetNode("/root/MainScreen/SubViewportContainer/SubViewport/Root/Hallway/MiddleLane");

        lane.GetChildren().ToList().ForEach(attack => attack.QueueFree());

        var newEnemy = enemies[_enemyTracker].Instantiate<Enemy>();
        var tween = CreateTween();
        tween.TweenProperty(newEnemy, "progress_ratio", .55, .1).SetDelay(.1);
        lane.AddChild(newEnemy);

        tween.TweenCallback(Callable.From(() => newEnemy.EnterBattle(null))).SetDelay(.1);
    }
}