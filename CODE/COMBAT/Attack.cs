using Godot;

public partial class Attack : PathFollow2D
{
	[Export]
	private PackedScene _AttackScene;

	public enum Style
	{
		OFFENSIVE, DEFENSIVE, COUNTERING
	}

	public float _pace;

	public Style _style;

	public Tween PROGRESS_TWEEN;
	public Tween PACE_TWEEN;

	public Node Attacker;

	public override void _Process(double delta)
	{
		if (PROGRESS_TWEEN == null || !PROGRESS_TWEEN.IsValid())
		{
			ProgressRatio += (float)delta * _pace;
		}

		if (_style == Style.OFFENSIVE)
		{
			GetNode<Sprite2D>("Sprite").Texture = ResourceLoader.Load<Texture2D>("res://ART/VISUALS/Red.png");
		}
		if (_style == Style.DEFENSIVE)
		{
			GetNode<Sprite2D>("Sprite").Texture = ResourceLoader.Load<Texture2D>("res://ART/VISUALS/Blue.png");
		}
		if (_style == Style.COUNTERING)
		{
			GetNode<Sprite2D>("Sprite").Texture = ResourceLoader.Load<Texture2D>("res://ART/VISUALS/Green.png");
		}
	}

	public void ChangeStyle(Area2D collidingAttack)
	{
		if (collidingAttack.GetCollisionLayerValue(CollisionMap.ENEMEY_JAB) && GetNode<Area2D>("Area2D").GetCollisionLayerValue(CollisionMap.PLAYER_JAB)
			&& !collidingAttack.IsQueuedForDeletion())
		{
			if (_style == Style.DEFENSIVE)
			{
				collidingAttack.GetParent().QueueFree();
				if (!IsQueuedForDeletion())
					QueueFree();
			}

			if (_style == Style.COUNTERING)
			{
				collidingAttack.GetParent().QueueFree();
				CustomSignals._Instance.EmitSignal(CustomSignals.SignalName.SuccessfulAttack, 10);
				CustomSignals._Instance.EmitSignal(CustomSignals.SignalName.RecoverStamina);
			}

			if (_style == Style.OFFENSIVE)
			{
				if (!IsQueuedForDeletion())
					QueueFree();
			}
		}
	}
}
