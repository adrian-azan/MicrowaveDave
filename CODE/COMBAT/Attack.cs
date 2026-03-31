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

	public void Blocked()
	{
		//TODO: make a path that goes behind the attack.
		// put the attack on that path
		// delete the path
		// delete the attack
	}

	public void ChangeStyle(Area2D collidingAttack2D)
	{
		if (collidingAttack2D.GetCollisionLayerValue(CollisionMap.ENEMEY_JAB) && GetNode<Area2D>("Area2D").GetCollisionLayerValue(CollisionMap.PLAYER_JAB)
			&& !collidingAttack2D.IsQueuedForDeletion())
		{
			if (_style == Style.DEFENSIVE)
			{
				AudioManager._Instance.Block();
				CreateTween().TweenProperty(this, "scale", Scale * 1.3f, .1);
				CreateTween().TweenProperty(this, "scale", Scale, .1).SetDelay(.1);

				Path2D blowBackPath = new Path2D();
				
				GetParent().GetParent().AddChild(blowBackPath);
				Curve2D actualPath = new Curve2D();
				actualPath.AddPoint(new Vector2((GetParent() as Path2D).Position.X, Position.Y));
				actualPath.AddPoint(new Vector2(Tools.rng.RandfRange(-500, 500), 1000));
				blowBackPath.Curve = actualPath;
				CallDeferred(MethodName.Reparent, blowBackPath);
		
				GetNode<Area2D>("Area2D").CollisionLayer = 0;

				_pace *= 4f;
				ProgressRatio = 0;
				
				CreateTween().TweenCallback(Callable.From(() => GetParent().QueueFree())).SetDelay(5);
			
				collidingAttack2D.GetParent().QueueFree();
			}

			if (_style == Style.COUNTERING)
			{
				collidingAttack2D.GetParent().QueueFree();
				CustomSignals._Instance.EmitSignal(CustomSignals.SignalName.SuccessfulAttack, 10);
				CustomSignals._Instance.EmitSignal(CustomSignals.SignalName.RecoverStamina);
			}

			if (_style == Style.OFFENSIVE)
			{
				if (!IsQueuedForDeletion())
				{
					Visible = false;
					GetNode<Area2D>("Area2D").SetDeferred("monitorable", false);
					GetNode<Area2D>("Area2D").SetDeferred("monitoring", false);
					CreateTween().TweenCallback(Callable.From(() => QueueFree())).SetDelay(DefenseLanes._staminaRecoveryRate);
				}
			}
		}
	}
}
