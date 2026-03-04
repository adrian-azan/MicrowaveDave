using Godot;

using System.Linq;

public partial class AttackLanes : Lanes
{
	public override void _Ready()
	{
		base._Ready();
		_lanes = Tools.GetChildren<Path2D>(this);
		CustomSignals._Instance.Reset += Reset;
	}

	public void AttackedByPlayer(Area2D incomingAttack)
	{
		incomingAttack.GetParent().QueueFree();

		CustomSignals._Instance.EmitSignal(CustomSignals.SignalName.SuccessfulAttack, 10);
	}

	public void Reset()
	{
		foreach (var lane in _lanes)
		{
			foreach (var attack in Tools.GetChildren<Attack>(lane))
			{
				attack.QueueFree();
			}
		}
	}
}
