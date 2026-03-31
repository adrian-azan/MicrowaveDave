using Godot;
using System;

public partial class CustomSignals : Node
{
	public static CustomSignals _Instance;

	public override void _Ready()
	{
		base._Ready();

		_Instance = this;
	}

	[Signal]
	public delegate void SwapLevelEventHandler(LevelLoader.LEVEL level);
	
	[Signal]
	public delegate void UpdateLightsEventHandler();

	[Signal]
	public delegate void UpdateShowTopEventHandler(bool visible);

	[Signal]
	public delegate void SuccessfulAttackEventHandler(int amount);

	[Signal]
	public delegate void SuccessfulAttackEnemyEventHandler();

	[Signal]
	public delegate void RecoverStaminaEventHandler();
	[Signal]
	public delegate void RecoverHealthEventHandler(int amount);
	
	[Signal]
	public delegate void IncreaseHealthEventHandler(int amount);

	[Signal]
	public delegate void EnemyKilledEventHandler();

	[Signal]
	public delegate void HideLanesEventHandler();

	[Signal]
	public delegate void RemoveAttackFromLaneEventHandler(Attack attack, bool enemyAttack);

	[Signal]
	public delegate void ResetEventHandler();
	
	[Signal]
	public delegate void StopEventHandler();
}
