using System.Linq;
using Godot;
using Godot.Collections;

public partial class DefenseLanes : Lanes
{
	public int _stamina;
	private int _maxStamina;
	private int _health;
	private int _maxHealth;
	public Array<Sprite2D> _staminaIndicators;
	
	
	public static float _staminaRecoveryRate;

	private PackedScene AttackScene;

	public override void _Ready()
	{
		base._Ready();
		_staminaIndicators = new Array<Sprite2D>(GetChildren().Where(node => node is Sprite2D).Cast<Sprite2D>());
		//TODO: Candidate for timer service
		
		Reset();
		
		
		AttackScene = ResourceLoader.Load<PackedScene>(Constants.ATTACK_SCENE);
		

		CustomSignals._Instance.RecoverStamina += RecoverStamina;
		CustomSignals._Instance.Reset += Reset;
		CustomSignals._Instance.RecoverHealth += RecoverHealth;
		CustomSignals._Instance.IncreaseHealth += IncreaseHealth;
		
		
		var defenseArea = GetNode<Area2D>("Core/PlayerDefense/Area2D");
		defenseArea.AreaEntered += DefensiveStyleEventHandler;
		defenseArea.AreaExited += DefaultStyleEventHandler;
		
		
	}

	public void Reset()
	{
		_stamina = 1;
		_maxStamina = 2;
		_health = 3;
		_maxHealth = 3;
		_staminaRecoveryRate = 1f;
	}

	public override void _Process(double delta)
	{
		if (_health <= 0)
			return;
		
		Attack attackInstance = null;
		
		GetNode<Label>("Label").Text = $"{_health}";
		
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
		
		if (_stamina < _maxStamina - Count())
			RecoverStamina();
		CalculateStaminaIndicators();
	}

	public void RecoverStamina()
	{
			_stamina += 1;
	}

	private void RecoverHealth(int amount)
	{
		_health += amount;
		_health = int.Clamp(_health, 0, _maxHealth);
	}
	
	private void IncreaseHealth(int amount)
	{
		_maxHealth += amount;
		_health = _maxHealth;
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
		_health -= 1;

		if (_health <= 0 && GetNodeOrNull("../GameOver") == null)
		{
			GameOver gameOverScreen = ResourceLoader.Load<PackedScene>("res://SCENES/GameOver.tscn").Instantiate() as GameOver;
			
			AddSibling(gameOverScreen);
			gameOverScreen.Show();
		}
	}
}
