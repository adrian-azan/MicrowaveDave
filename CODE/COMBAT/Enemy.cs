using Godot;
using Godot.Collections;
using System.Threading.Tasks;

public partial class Enemy : PathFollow3D
{
	public enum STATE
	{
		ATTACKING,
		IDLE
	};

	[Export]
	public Lanes.LANES _designatedLane;

	public STATE _state;

	protected AttackLanes _attackLanes;

	protected int _health;
	public float _pace;
	[Export]
	protected float _attackCoolDown;

	protected Tween PROGRESS_TWEEN;
	protected Tween PACE_TWEEN;

	Dictionary<string, Tween> _tweens = new Dictionary<string, Tween>();
	
	//Don't have a use case yet, but could be handy to have enemies with custom end of life methods
	private Callable EndTask;
	
	AudioStreamPlayer _audioPlayer;

	public override void _Ready()
	{
		_attackLanes = GetTree().GetFirstNodeInGroup("AttackLane") as AttackLanes;
		ProgressRatio = .3f;
		_pace = .04f;
		_state = STATE.IDLE;

		_health = 1000;

		Bounce();
		GetNode<Timer>("Timer").SetWaitTime(_attackCoolDown);
	}


	public void Die()
	{
		if (_audioPlayer != null)
			_audioPlayer.Stop();
		
		_attackLanes.ClearLanes(this);
		CustomSignals._Instance.EmitSignal(CustomSignals.SignalName.EnemyKilled);
		AudioManager._Instance.EnemyDeath();
		
		Path3D deathPath = new Path3D();
		deathPath.Curve = ResourceLoader.Load<Curve3D>("res://SCENES/Battle System/EnemyDeathCurve.tres");
		deathPath.Curve.SetPointPosition(0, GlobalPosition);
		
		deathPath.Rotation = new Vector3(0, Tools.rng.RandfRange(0f, 40f), 0);
		
		GetTree().Root.AddChild(deathPath);

		ProgressRatio = 0;
		CreateTween().TweenProperty(this, "progress_ratio", 1, 1);
		CreateTween().TweenProperty(this, "rotation", new Vector3(1800,1800,0), 4);
		CallDeferred(MethodName.Reparent, deathPath);

		CreateTween().TweenCallback(Callable.From(() =>
		{
			QueueFree();
			deathPath.QueueFree();
		} )).SetDelay(5);

		GetNode<Timer>("Timer").Paused = true;
		
		if(EndTask.Delegate != null)
			EndTask.Call();
	}

	public override void _ExitTree()
	{
		base._ExitTree();	
		if (_audioPlayer != null)
			_audioPlayer.Stop();
	}

	public bool Dead()
	{
		return _health <= 0;
	}

	public void TakeDamage(int amount)
	{
		_health -= amount;
		AudioManager._Instance.Punch();

		//Get Hit animation
		var side = new Array() { -1, 1 };

		float sideAngle = Tools.rng.RandfRange(-50, -40);
		sideAngle = sideAngle * side.PickRandom().AsInt32();

		var tween = CreateTween();
		tween.SetTrans(Tween.TransitionType.Bounce);
		tween.TweenProperty(this, "rotation", new Vector3(Mathf.DegToRad(-50), Mathf.DegToRad(sideAngle), 0), .25);
		tween.TweenProperty(this, "rotation", new Vector3(0, 0, 0), .5);


		if (_tweens.TryGetValue("scale",out _) == false || !_tweens["scale"].IsValid())
		{
			_tweens["scale"] = CreateTween();
			Vector3 currentScale = Scale;
			Vector3 stretched = new Vector3(currentScale.X, currentScale.Y * 1.3f, currentScale.Z);

			_tweens["scale"].TweenProperty(this, "scale", stretched, .1);
			_tweens["scale"].TweenProperty(this, "scale", currentScale, .1).SetDelay(.1);
			
		}
	}

	public void EnterBattle(Area3D area)
	{
		_state = STATE.ATTACKING;
		_audioPlayer = AudioManager._Instance.EnemyStart();
	}

	public Attack AttackPlayer()
	{
		var attackScene = ResourceLoader.Load<PackedScene>(Constants.ATTACK_SCENE);
		var attackInstance = attackScene.Instantiate<Attack>();

		attackInstance.Attacker = this;
		_attackLanes.AddAttackToLane(attackInstance, _designatedLane);

		return attackInstance;
	}

	public void Bounce()
	{
		var tween = CreateTween();
		tween.SetTrans(Tween.TransitionType.Bounce);
		tween.TweenProperty(GetNode<MeshInstance3D>("MeshInstance3D"), "position", new Vector3(0, .3f, 0), .25f).AsRelative();
		tween.TweenProperty(GetNode<MeshInstance3D>("MeshInstance3D"), "position", new Vector3(0, -.3f, 0), .25f).AsRelative();

		tween.TweenCallback(Callable.From(() => Bounce()));
	}
}
