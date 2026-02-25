using Godot;
using System;
using System.Linq;
using Godot.Collections;

public partial class Level1_EnemyGenerator : Node3D
{
	private String DEBUG_PhaseString = "[b]Level {0}:[/b] Starting [color=red]{1}[/color]";

	private Hallway _hallway;
	private AnimationPlayer _animationPlayer;

	private enum Phase
	{
		ONE,
		TWO,
		THREE,
		FOUR,
		FIVE,
		SIX,
		SEVEN,
		EIGHT,
		NINE,
		TEN,
		NONE
	};
	
	private Phase _currentPhase;

	[Export] private Array<PackedScene> _availableEnemies;

	private Timer COOL_DOWN;

	public override void _Ready()
	{
		_hallway = GetNode<Hallway>("Hallway");
		_animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");

		_animationPlayer.Play("Level_1/LEVEL_ONE");
		_animationPlayer.Seek(8, true);
		_animationPlayer.PlaySectionWithMarkers("Level_1/LEVEL_ONE", "PhaseSix");
	
		CustomSignals._Instance.SuccessfulAttack += DamageEnemy;
		CustomSignals._Instance.EnemyKilled += CheckIfPhaseFinishedEarly;
	}

	public override void _Process(double delta)
	{
		GetNode<Label3D>("Label3D").Text = Math.Round(_animationPlayer.CurrentAnimationPosition, 0).ToString();
	}

	public void CheckIfPhaseFinishedEarly()
	{
		if (_hallway.IsEmpty())
		{
			AdvancePhase();
		}
	}


	/*
	 * Ok lets explain this a bit. SO, I figure the best time to check
	 * if we should move onto the next phase, would be when an enemy dies. Enemy dies
	 * might be the last one, check if your empty. So the 'CheckIfPhaseFinishedEarly'
	 * happens when any enemy dies, and if the hallway is empty we advance.
	 * THE PROBLEM: I noticed that if I killed 2 enemies in the same phase very quickly
	 * it would double advance phases before the hallway could properlly fill our empty out.
	 * just an issue with deleing nodes and checking statuses. So a Jank solution was to just
	 * add a little cool down for the generator to have. Wait a few seconds before allowing
	 * another phase advancement 
	 */
	public void AdvancePhase()
	{
		Timer coolDown = GetNode<Timer>("PhaseAdvanceCoolDown");
		if (coolDown.TimeLeft > 0.1)
			return;
		coolDown.Start();
	
		
		_currentPhase++;
		Logging.PrintInfo("Advancing Phase",$"_currentPhase {_currentPhase}");
		switch (_currentPhase)
		{
			case Phase.TWO:
				_animationPlayer.PlaySectionWithMarkers("Level_1/LEVEL_ONE", "PhaseTwo");
				break;
			case Phase.THREE:
				_animationPlayer.PlaySectionWithMarkers("Level_1/LEVEL_ONE", "PhaseThree");
				break;
			case Phase.FOUR:
				_animationPlayer.PlaySectionWithMarkers("Level_1/LEVEL_ONE", "PhaseFour");
				break;
			case Phase.FIVE:
				_animationPlayer.PlaySectionWithMarkers("Level_1/LEVEL_ONE", "PhaseFive");
				break;
			case Phase.SIX:
				_animationPlayer.PlaySectionWithMarkers("Level_1/LEVEL_ONE", "PhaseSix");
				break;
			case Phase.SEVEN:
				_animationPlayer.PlaySectionWithMarkers("Level_1/LEVEL_ONE", "PhaseSeven");
				break;
			case Phase.EIGHT:
				_animationPlayer.PlaySectionWithMarkers("Level_1/LEVEL_ONE", "PhaseEight");
				break;
			case Phase.NINE:
				_animationPlayer.PlaySectionWithMarkers("Level_1/LEVEL_ONE", "PhaseNine");
				break;
			case Phase.TEN:
				_animationPlayer.PlaySectionWithMarkers("Level_1/LEVEL_ONE", "PhaseTen");
				break;
		}
	}
	
	public void DamageEnemy(int amount)
	{
		Array<Enemy> enemiesInLanes = new Array<Enemy>();
		enemiesInLanes.Add(_hallway._lanes[1].GetChildCount() > 0 ? _hallway._lanes[1].GetChild<Enemy>(0) : null);
		enemiesInLanes.Add(_hallway._lanes[0].GetChildCount() > 0 ? _hallway._lanes[0].GetChild<Enemy>(0) : null);
		enemiesInLanes.Add(_hallway._lanes[2].GetChildCount() > 0 ? _hallway._lanes[2].GetChild<Enemy>(0) : null);

		enemiesInLanes = new Array<Enemy>(enemiesInLanes.Where(enemy => enemy != null));

		if (enemiesInLanes.Count == 0)
			return;

		enemiesInLanes[0].TakeDamage(amount);

		if (enemiesInLanes[0].Dead())
		{
			enemiesInLanes[0].QueueFree();
			enemiesInLanes.RemoveAt(0);
		}
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
		Logging.PrintInfo("Level 1:", "Starting PhaseOne");
		_currentPhase = Phase.ONE;
		
		var simpleStraight = _availableEnemies[0].Instantiate<Enemy>();
		simpleStraight._designatedLane = Lanes.LANES.MIDDLE;
		_hallway._lanes[(int)Lanes.LANES.MIDDLE].AddChild(simpleStraight);
	}

	public void PhaseTwo()
	{
		Logging.PrintInfo("Level 1:", "Starting PhaseTwo");
		_currentPhase = Phase.TWO;

		var simpleStraight = _availableEnemies[0].Instantiate<Enemy>();
		var simpleStraight_2 = _availableEnemies[0].Instantiate<Enemy>();
		simpleStraight._designatedLane = Lanes.LANES.MIDDLE;
		simpleStraight_2._designatedLane = Lanes.LANES.LEFT;

		_hallway._lanes[(int)Lanes.LANES.MIDDLE].AddChild(simpleStraight);

		CreateTween().TweenCallback(Callable.From(() => _hallway._lanes[(int)Lanes.LANES.LEFT].AddChild(simpleStraight_2))).SetDelay(5);
	}

	public void PhaseThree()
	{
		Logging.PrintInfo("Level 1:", "Starting PhaseThree");
		_currentPhase = Phase.THREE;

		var simpleStraight2 = _availableEnemies[0].Instantiate<Enemy>();
		var simpleStraight = _availableEnemies[0].Instantiate<Enemy>();

		simpleStraight2._designatedLane = Lanes.LANES.RIGHT;
		simpleStraight._designatedLane = Lanes.LANES.LEFT;

		_hallway._lanes[(int)Lanes.LANES.RIGHT].AddChild(simpleStraight2);
		_hallway._lanes[(int)Lanes.LANES.LEFT].AddChild(simpleStraight);
	}

	public void PhaseFour()
	{
		Logging.PrintInfo("Level 1:", "Starting PhaseFour");
		_currentPhase = Phase.FOUR;
		
		var simpleStraight = _availableEnemies[0].Instantiate<Enemy>();
		var simpleStraight1 = _availableEnemies[0].Instantiate<Enemy>();
		var simpleStraight2 = _availableEnemies[0].Instantiate<Enemy>();

		simpleStraight._designatedLane = Lanes.LANES.RIGHT;
		simpleStraight1._designatedLane = Lanes.LANES.LEFT;
		simpleStraight2._designatedLane = Lanes.LANES.MIDDLE;

		_hallway._lanes[(int)Lanes.LANES.LEFT].AddChild(simpleStraight1);
		_hallway._lanes[(int)Lanes.LANES.RIGHT].AddChild(simpleStraight);
		CreateTween().TweenCallback(Callable.From(() => _hallway._lanes[(int)Lanes.LANES.MIDDLE].AddChild(simpleStraight2))).SetDelay(20);
	}

	public void PhaseFive()
	{
		Logging.PrintInfo("Level 1:", "Starting PhaseFive");
		_currentPhase = Phase.FIVE;

		var curve = _availableEnemies[1].Instantiate<Enemy>();
		curve._designatedLane = Lanes.LANES.MIDDLE;
		
		_hallway._lanes[(int)Lanes.LANES.MIDDLE].AddChild(curve);
	}

	public void PhaseSix()
	{
		Logging.PrintInfo("Level 1:", "Starting PhaseSix");
		_currentPhase = Phase.SIX;

		var curve = _availableEnemies[1].Instantiate<Enemy>();
		curve._designatedLane = Lanes.LANES.MIDDLE;
		var straight = _availableEnemies[0].Instantiate<Enemy>();
		straight._designatedLane = Lanes.LANES.LEFT;
		
		_hallway._lanes[(int)Lanes.LANES.MIDDLE].AddChild(curve);
		_hallway._lanes[(int)Lanes.LANES.LEFT].AddChild(straight);
	}

	public void PhaseSeven()
	{
		Logging.PrintInfo("Level 1:", "Starting PhaseSeven");
		_currentPhase = Phase.SEVEN;

		var curve = _availableEnemies[1].Instantiate<Enemy>();
		var straight = _availableEnemies[0].Instantiate<Enemy>();
		
		curve._designatedLane = Lanes.LANES.LEFT;
		straight._designatedLane = Lanes.LANES.MIDDLE;
		
		_hallway._lanes[(int)Lanes.LANES.LEFT].AddChild(curve);
		_hallway._lanes[(int)Lanes.LANES.MIDDLE].AddChild(straight);
	}

	public void PhaseEight()
	{
		Logging.PrintInfo("Level 1:", "Starting PhaseEight");
		_currentPhase = Phase.EIGHT;

		var curve = _availableEnemies[1].Instantiate<Enemy>();
		var straight = _availableEnemies[0].Instantiate<Enemy>();
		var straight2 = _availableEnemies[0].Instantiate<Enemy>();
		
		curve._designatedLane = Lanes.LANES.MIDDLE;
		straight._designatedLane = Lanes.LANES.LEFT;
		straight2._designatedLane = Lanes.LANES.RIGHT;
		
		CreateTween().TweenCallback(Callable.From(() => _hallway._lanes[(int)Lanes.LANES.MIDDLE].AddChild(curve))).SetDelay(20);
		_hallway._lanes[(int)Lanes.LANES.LEFT].AddChild(straight);
		_hallway._lanes[(int)Lanes.LANES.RIGHT].AddChild(straight2);
	}

	public void PhaseNine()
	{
		Logging.PrintInfo("Level 1:", "Starting PhaseNine");
		_currentPhase = Phase.NINE;

		var curve = _availableEnemies[1].Instantiate<Enemy>();
		var curve2 = _availableEnemies[1].Instantiate<Enemy>();
		
		curve._designatedLane = Lanes.LANES.LEFT;
		curve2._designatedLane = Lanes.LANES.RIGHT;
		
		_hallway._lanes[(int)Lanes.LANES.RIGHT].AddChild(curve2);
		CreateTween().TweenCallback(Callable.From(() => _hallway._lanes[(int)Lanes.LANES.LEFT].AddChild(curve))).SetDelay(.5);
	}
	
	public void PhaseTen()
	{
		if (!_hallway.IsEmpty())
		{
			Logging.PrintInfo("Level 1:", "Fights not finished.");
			_animationPlayer.Stop();
			_animationPlayer.PlaySectionWithMarkers("LEVEL_ONE","loop");
			return;
		}
		
		_currentPhase = Phase.TEN;

		Logging.PrintInfo("Level 1:", "Starting PhaseTen");

		_animationPlayer.Stop(true);
		_animationPlayer.Play("LEVEL_ONE_STORE");
	}
}
