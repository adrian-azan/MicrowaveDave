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
        NONE
    };
    
    private Phase _currentPhase;

    [Export] private Array<PackedScene> _availableEnemies;

    private Timer COOL_DOWN;

    public override void _Ready()
    {
        _hallway = GetNode<Hallway>("Hallway");
        _animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");

        _animationPlayer.Play("LEVEL_ONE");
        _animationPlayer.PlaySectionWithMarkers("LEVEL_ONE", "PhaseFive");

        CustomSignals._Instance.SuccesfulAttackSignal += DamageEnemy;
        CustomSignals._Instance.EnemyKilled += CheckIfPhaseFinishedEarly;
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
                _animationPlayer.PlaySectionWithMarkers("LEVEL_ONE", "PhaseTwo");
                break;
            case Phase.THREE:
                _animationPlayer.PlaySectionWithMarkers("LEVEL_ONE", "PhaseThree");
                break;
            case Phase.FOUR:
                _animationPlayer.PlaySectionWithMarkers("LEVEL_ONE", "PhaseFour");
                break;
            case Phase.FIVE:
                _animationPlayer.PlaySectionWithMarkers("LEVEL_ONE", "PhaseFive");
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
       GD.PrintRich(String.Format(DEBUG_PhaseString, "1","PhaseOne"));
        var simpleStraight = _availableEnemies[0].Instantiate<Enemy>();
        simpleStraight._designatedLane = Lanes.LANES.MIDDLE;

        _hallway._lanes[1].AddChild(simpleStraight);

        var simpleStraight2 = _availableEnemies[0].Instantiate<Enemy>();
        simpleStraight2._designatedLane = Lanes.LANES.MIDDLE;

        _hallway._lanes[1].AddChild(simpleStraight2);
        
    }

    public void PhaseTwo()
    {
        GD.PrintRich(String.Format(DEBUG_PhaseString, "1","PhaseTwo"));

        var simpleStraight = _availableEnemies[0].Instantiate<Enemy>();
        var simpleStraight_2 = _availableEnemies[0].Instantiate<Enemy>();
        simpleStraight._designatedLane = Lanes.LANES.MIDDLE;
        simpleStraight_2._designatedLane = Lanes.LANES.LEFT;

        _hallway._lanes[1].AddChild(simpleStraight);

        CreateTween().TweenCallback(Callable.From(() => _hallway._lanes[0].AddChild(simpleStraight_2))).SetDelay(3);
    }

    public void PhaseThree()
    {
        GD.PrintRich(String.Format(DEBUG_PhaseString, "1","PhaseThree"));

        var simpleCurve = _availableEnemies[1].Instantiate<Enemy>();
        var simpleStraight = _availableEnemies[0].Instantiate<Enemy>();

        simpleCurve._designatedLane = Lanes.LANES.MIDDLE;
        simpleStraight._designatedLane = Lanes.LANES.LEFT;

        _hallway._lanes[1].AddChild(simpleCurve);
        _hallway._lanes[0].AddChild(simpleStraight);
    }

    public void PhaseFour()
    {
        GD.PrintRich(String.Format(DEBUG_PhaseString, "1","PhaseFour"));

        var simpleStraight = _availableEnemies[0].Instantiate<Enemy>();
        var simpleCurve = _availableEnemies[1].Instantiate<Enemy>();

        simpleStraight._designatedLane = Lanes.LANES.MIDDLE;
        simpleCurve._designatedLane = Lanes.LANES.LEFT;

        _hallway._lanes[0].AddChild(simpleCurve);
        _hallway._lanes[1].AddChild(simpleStraight);
    }

    public void PhaseFive()
    {
        GD.PrintRich(String.Format(DEBUG_PhaseString, "1","PhaseFive"));

        _animationPlayer.Stop(true);
        _animationPlayer.Play("LEVEL_ONE_STORE");
    }
}
