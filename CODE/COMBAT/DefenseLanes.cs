using System.Linq;
using Godot;
using Godot.Collections;

public partial class DefenseLanes : Lanes
{
	public int _stamina;
	public Array<Sprite2D> _staminaIndicators;

	public Timer _staminaRecoveryTimer;
	
	[Export]
	public float _staminaRecoveryRate;

	public override void _Ready()
	{
		base._Ready();
		_stamina = 2;
		_staminaIndicators = new Array<Sprite2D>(GetChildren().Where(node => node is Sprite2D).Cast<Sprite2D>());
		_staminaRecoveryTimer = GetNode<Timer>("StaminaRecovery");

		CustomSignals._Instance.RecoverStaminaSignal += RecoverStamina;

		GetNode<Area2D>("Core/PlayerDefense/Area2D").AreaEntered += DefensiveStyle;
		GetNode<Area2D>("Core/PlayerDefense/Area2D").AreaExited += DefaultStyle;
	}

	public override void _Process(double delta)
	{
		float felta = (float)delta;

		GetNode<Label>("Label").Text = $"Stamina: {_staminaRecoveryTimer.WaitTime:0.##}";

		if (Input.IsActionJustPressed("LeftAttack") && _stamina > 0)
		{
			var attackScene = ResourceLoader.Load<PackedScene>(Constants.ATTACK_SCENE);
			var attackInstance = attackScene.Instantiate<Attack>();

			attackInstance._pace = .4f;
			attackInstance.GetNode<Area2D>("Area2D").SetCollisionLayerValue(6, true);
			attackInstance.GetNode<Area2D>("Area2D").SetCollisionLayerValue(2, false);

			AddAttackToLane(attackInstance, LANES.LEFT);

			_stamina -= 1;
		}

		if (Input.IsActionJustPressed("MiddleAttack") && _stamina > 0)
		{
			var attackScene = ResourceLoader.Load<PackedScene>(Constants.ATTACK_SCENE);
			var attackInstance = attackScene.Instantiate<Attack>();

			attackInstance._pace = .4f;
			attackInstance.GetNode<Area2D>("Area2D").SetCollisionLayerValue(6, true);
			attackInstance.GetNode<Area2D>("Area2D").SetCollisionLayerValue(2, false);

			AddAttackToLane(attackInstance, LANES.MIDDLE);

			_stamina -= 1;
		}

		if (Input.IsActionJustPressed("RightAttack") && _stamina > 0)
		{
			var attackScene = ResourceLoader.Load<PackedScene>(Constants.ATTACK_SCENE);
			var attackInstance = attackScene.Instantiate<Attack>();

			attackInstance._pace = .4f;
			attackInstance.GetNode<Area2D>("Area2D").SetCollisionLayerValue(6, true);
			attackInstance.GetNode<Area2D>("Area2D").SetCollisionLayerValue(2, false);

			AddAttackToLane(attackInstance, LANES.RIGHT);

			_stamina -= 1;
		}

		foreach (var staminaIndicator in _staminaIndicators)
		{
			staminaIndicator.Visible = false;
		}
		
		for (int i = 0; i < _stamina; i++)
		{
			_staminaIndicators[i].Visible = true;
		}

		_staminaRecoveryTimer = GetNode<Timer>("StaminaRecovery");
		if (_stamina == 2)
		{
			_staminaRecoveryTimer.Paused = true;
			_staminaRecoveryTimer.SetWaitTime(_staminaRecoveryRate);
		}
		else
		{

			_staminaRecoveryTimer.Paused = false;
		}
	}

	public void RecoverStamina()
	{
		if (_stamina < 2)
			_stamina += 1;
	}
	
	public void DefaultStyle(Area2D area)
	{
		(area.GetParent() as Attack)._style = Attack.Style.OFFENSIVE;
	}

	public void DefensiveStyle(Area2D area)
	{
		(area.GetParent() as Attack)._style = Attack.Style.DEFENSIVE;
	}

	public void CollisionWithHeart(Area2D incomingAttack)
	{
		incomingAttack.GetParent().QueueFree();
	}
}
