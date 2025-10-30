using Godot;
using System;
using Godot.Collections;

public partial class Level1_EnemyGenerator : Node3D
{
	private Hallway _hallway;
    private AnimationPlayer _animationPlayer;
    
    [Export]
    private Array<PackedScene> _availableEnemies;
	public override void _Ready()
	{
		_hallway = GetNode<Hallway>("Hallway");
        _animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
        
        _animationPlayer.Play("LEVEL_ONE");
    }

	   public void TutorialPhaseOne()
    {
        _hallway._lanes[1].AddChild(_availableEnemies[0].Instantiate<Enemy>());

        GetNode<AnimationPlayer>("AnimationPlayer").Pause();
    }

    public void TutorialPhaseTwo()
    {
        var curve = _availableEnemies[1].Instantiate<Enemy>();

        curve._designatedLane = Lanes.LANES.MIDDLE;

        _hallway._lanes[1].AddChild(curve);

        GetNode<AnimationPlayer>("AnimationPlayer").Pause();
    }

    public void TutorialPhaseFour()
    {
        var normal = _availableEnemies[0].Instantiate<Enemy>();
        var curve = _availableEnemies[1].Instantiate<Enemy>();

        normal._designatedLane = Lanes.LANES.MIDDLE;
        curve._designatedLane = Lanes.LANES.LEFT;

        _hallway._lanes[1].AddChild(normal);
        _hallway._lanes[0].AddChild(curve);

        GetNode<AnimationPlayer>("AnimationPlayer").Pause();
    }

    public void PhaseOne()
    {
        var simpleStraight = _availableEnemies[0].Instantiate<Enemy>();
        simpleStraight._designatedLane = Lanes.LANES.MIDDLE;

        _hallway._lanes[1].AddChild(simpleStraight);

        var simpleStraight2 = _availableEnemies[0].Instantiate<Enemy>();
        simpleStraight2._designatedLane = Lanes.LANES.MIDDLE;

        _hallway._lanes[1].AddChild(simpleStraight2);
    }

    public void PhaseTwo()
    {
        var simpleStraight = _availableEnemies[0].Instantiate<Enemy>();
        var simpleStraight_2 = _availableEnemies[0].Instantiate<Enemy>();
        simpleStraight._designatedLane = Lanes.LANES.MIDDLE;
        simpleStraight_2._designatedLane = Lanes.LANES.LEFT;

        _hallway._lanes[1].AddChild(simpleStraight);

        CreateTween().TweenCallback(Callable.From(() => _hallway._lanes[0].AddChild(simpleStraight_2))).SetDelay(10);
    }

    public void PhaseThree()
    {
        var simpleCurve = _availableEnemies[1].Instantiate<Enemy>();
        var simpleStraight_2 = _availableEnemies[0].Instantiate<Enemy>();

        simpleCurve._designatedLane = Lanes.LANES.MIDDLE;
        simpleStraight_2._designatedLane = Lanes.LANES.LEFT;

        _hallway._lanes[1].AddChild(simpleCurve);
        _hallway._lanes[0].AddChild(simpleStraight_2);
    }

    public void PhaseFour()
    {
        var simpleStraight = _availableEnemies[0].Instantiate<Enemy>();
        var simpleCurve = _availableEnemies[1].Instantiate<Enemy>();
        var simpleStraight_2 = _availableEnemies[0].Instantiate<Enemy>();

        simpleStraight._designatedLane = Lanes.LANES.MIDDLE;
        simpleCurve._designatedLane = Lanes.LANES.LEFT;
        simpleStraight_2._designatedLane = Lanes.LANES.RIGHT;

        _hallway._lanes[0].AddChild(simpleCurve);
        _hallway._lanes[1].AddChild(simpleStraight);
        _hallway._lanes[2].AddChild(simpleStraight_2);
    }
}
