using System.Linq;
using Godot;
using Godot.Collections;

public partial class DefenseLanes : Lanes
{
	public int _stamina;
	private int _maxStamina;
	public Array<Sprite2D> _staminaIndicators;

	//TODO: good timer tool candidate
	public Timer _staminaRecoveryTimer;
	
	[Export]
	public float _staminaRecoveryRate;

	private PackedScene AttackScene;

	public override void _Ready()
	{
		base._Ready();
		_stamina = 2;
		_maxStamina = 2;
		_staminaIndicators = new Array<Sprite2D>(GetChildren().Where(node => node is Sprite2D).Cast<Sprite2D>());
		//TODO: Candidate for timer service
		_staminaRecoveryTimer = GetNode<Timer>("StaminaRecovery");
		
		AttackScene = ResourceLoader.Load<PackedScene>(Constants.ATTACK_SCENE);
		

		CustomSignals._Instance.RecoverStamina += RecoverStamina;

		var defenseArea = GetNode<Area2D>("Core/PlayerDefense/Area2D");
		defenseArea.AreaEntered += DefensiveStyleEventHandler;
		defenseArea.AreaExited += DefaultStyleEventHandler;
	}

	public override void _Process(double delta)
	{
		Attack attackInstance = null;
		
		if (Input.IsActionJustPressed("LeftAttack") && _stamina > 0)
		{
			attackInstance = AttackScene.Instantiate<Attack>();
			AddAttackToLane(attackInstance, LANES.LEFT);
		}

		if (Input.IsActionJustPressed("MiddleAttack") && _stamina > 0)
		{
			attackInstance = AttackScene.Instantiate<Attack>();
			AddAttackToLane(attackInstance, LANES.MIDDLE);
		}

		if (Input.IsActionJustPressed("RightAttack") && _stamina > 0)
		{
			attackInstance = AttackScene.Instantiate<Attack>();
			AddAttackToLane(attackInstance, LANES.RIGHT);
		}


		if (attackInstance != null)
		{
			_stamina -= 1;
			attackInstance._pace = .4f;
			attackInstance.GetNode<Area2D>("Area2D").SetCollisionLayerValue(6, true);
			attackInstance.GetNode<Area2D>("Area2D").SetCollisionLayerValue(2, false);
		}
		
		CalculateStaminaIndicators();
	}

	public void RecoverStamina()
	{
		if (_stamina < _maxStamina)
			_stamina += 1;
	}

	private void CalculateStaminaIndicators()
	{
		foreach (var staminaIndicator in _staminaIndicators)
		{
			staminaIndicator.Visible = false;
		}
		
		for (int i = 0; i < _stamina; i++)
		{
			_staminaIndicators[i].Visible = true;
		}

		//TODO: Another good timer candidate
		//When we've reached our max, we should no longer recover stamina.
		//Pausing the timer ensures that when we use our first bit of stamina
		//Will always start the recovery timer from scratch
		if (_stamina == _maxStamina)
		{
			_staminaRecoveryTimer.Paused = true;
			_staminaRecoveryTimer.SetWaitTime(_staminaRecoveryRate);
		}
		else
		{
			_staminaRecoveryTimer.Paused = false;
		}
	}

	
	private void DefaultStyleEventHandler(Area2D area)
	{
		(area.GetParent() as Attack)._style = Attack.Style.OFFENSIVE;
	}

	private void DefensiveStyleEventHandler(Area2D area)
	{
		(area.GetParent() as Attack)._style = Attack.Style.DEFENSIVE;
	}

	private void CollisionWithHeart(Area2D incomingAttack)
	{
		incomingAttack.GetParent().QueueFree();
	}
}
