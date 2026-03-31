using Godot;
using Godot.Collections;
using System.Linq;

public partial class HallwayDisco : Hallway
{
	private Array<HallwayDiscoPiece> _pieces;

	private Array<Array<NeonLightPanel>> _bottomLights;
	private Array<Array<NeonLightPanel>> _topLights;

	public Timer _theBeat;

	public bool _showTop;

	public static float _discoLightFlickerChance = 0;

	private Array<Tween> _tweens;

	[Export]
	public NeonLightPanel.COLOR _primaryColor;

	[Export]
	public NeonLightPanel.COLOR _secondaryColor;

	public enum LightingStyle
	{
		QUADS,
		CHECKERED,
		TWOLANES,
		FOURLANES
	};

	public static LightingStyle _lightingStyle;

	public override void _Ready()
	{
		_pace = .1f;
		float totalPieces = 24;
		

		_lanes = new Array<Path3D>();
		_lanes.Add(GetNode<Path3D>("LeftLane"));
		_lanes.Add(GetNode<Path3D>("MiddleLane"));
		_lanes.Add(GetNode<Path3D>("RightLane"));
		
		_theBeat = GetNode<Timer>("Timer");
		_theBeat.Start();

		_pieces = Tools.GetChildren<HallwayDiscoPiece>(this);
		
		for (int i = 0; i < totalPieces; i++)
		{
			_pieces.Add(piecePackedScene.Instantiate() as HallwayDiscoPiece);
			GetNode("Path3D").AddChild(_pieces[i]);
			_pieces[i].ProgressRatio = (float)i / totalPieces;
		}
		
		_bottomLights = new Array<Array<NeonLightPanel>>();
		_topLights = new Array<Array<NeonLightPanel>>();
		_tweens = new Array<Tween>();

		foreach (Node3D piece in _pieces)
		{
			Array<NeonLightPanel> lights = Tools.GetChildren<NeonLightPanel>(piece.GetNode("FloorTiles"));

			_bottomLights.Add(lights.Slice(0, 5));
			_bottomLights.Add(lights.Slice(5, 5));
			_bottomLights.Add(lights.Slice(10, 5));

			_topLights.Add(lights.Slice(15, 5));
			_topLights.Add(lights.Slice(20, 5));
			_topLights.Add(lights.Slice(25, 5));
		}

		CustomSignals._Instance.UpdateLights += UpdateLights;
		CustomSignals._Instance.UpdateShowTop += UpdateTop;

		UpdateLights();
	}
	
	public override void _Process(double delta)
	{
		float felta = (float)delta;
		foreach (PathFollow3D piece in _pieces)
		{
			piece.ProgressRatio += felta * _pace;
		}
		

		Array<Array<Enemy>> Lanes = new Array<Array<Enemy>>();
		Lanes.Add(Tools.GetChildren<Enemy>(_lanes[1]));
		Lanes.Add(Tools.GetChildren<Enemy>(_lanes[0]));
		Lanes.Add(Tools.GetChildren<Enemy>(_lanes[2]));

		foreach (Array<Enemy> lane in Lanes)
		{
			if (lane.Count == 0 || (lane.First().ProgressRatio > .9 && lane.First().ProgressRatio < .95))
				continue;

			lane.First().ProgressRatio += felta * lane.First()._pace * _DEBUG_enemyPaceMultiplyer;
		}
	}

	public void Start()
	{
		int delay = 0;
		foreach (Node3D piece in _pieces.OrderByDescending(x => x.ProgressRatio))
		{
			piece.Visible = false;
			CreateTween().TweenCallback(Callable.From(() => piece.Visible = true)).SetDelay(.5);
		}

		CreateTween().TweenCallback(Callable.From(() => UpdateTop(true))).SetDelay(3);
		AudioManager._Instance.PlaySong("res://ART/SOUND/SONGS/Midnight Generation - Don_t Wait Up (Official Music Video).mp3");
	}

	public bool Busy()
	{
		return _tweens.Count > 0;
	}

	public void UpdateLights()
	{
		switch (_lightingStyle)
		{
			case LightingStyle.QUADS:
				Quads();
				break;

			case LightingStyle.CHECKERED:
				Checkered();
				break;

			case LightingStyle.FOURLANES:
				FourLanes();
				break;

			case LightingStyle.TWOLANES:
				TwoLanes();
				break;
		}
	}

	public void UpdateTop(bool visible)
	{
		foreach (Array<NeonLightPanel> piece in _topLights)
		{
			foreach (NeonLightPanel light in piece)
			{
				if (visible == true)
				{
					var tween = GetTree().CreateTween();
					tween.SetTrans(Tween.TransitionType.Bounce);

					light.Visible = true;
					tween.TweenProperty(light, "position", new Vector3(light.Position.X, 15, light.Position.Z), 0);
					tween.TweenProperty(light, "position", new Vector3(light.Position.X, 4, light.Position.Z), Tools.rng.RandfRange(1, 2));
					//tween.TweenProperty(light, "visible", true, 1.5);

					_tweens.Add(tween);
					tween.TweenCallback(Callable.From(() => _tweens.Remove(tween)));
				}
				else
				{
					var tween = GetTree().CreateTween();
					tween.SetTrans(Tween.TransitionType.Circ);

					tween.TweenProperty(light, "position", new Vector3(light.Position.X, 4, light.Position.Z), 0);
					tween.TweenProperty(light, "position", new Vector3(light.Position.X, 15, light.Position.Z), Tools.rng.RandfRange(1, 2));
					tween.TweenProperty(light, "visible", false, 2);

					_tweens.Add(tween);
					tween.TweenCallback(Callable.From(() => _tweens.Remove(tween)));
				}
			}
		}
	}

	public void ChangeColors(NeonLightPanel.COLOR primary, NeonLightPanel.COLOR secondary)
	{
	}

	public void TwoLanes()
	{
		for (int i = 0; i < _bottomLights.Count; i++)
		{
			_bottomLights[i][0].Change(_primaryColor);
			_bottomLights[i][1].Change(_primaryColor);
			_bottomLights[i][2].Change(_secondaryColor);
			_bottomLights[i][3].Change(_secondaryColor);

			_topLights[i][3].Change(_primaryColor);
			_topLights[i][2].Change(_primaryColor);
			_topLights[i][1].Change(_secondaryColor);
			_topLights[i][0].Change(_secondaryColor);
		}
	}

	public void FourLanes()
	{
		for (int i = 0; i < _bottomLights.Count; i++)
		{
			_bottomLights[i][0].Change(_primaryColor);
			_bottomLights[i][1].Change(_secondaryColor);
			_bottomLights[i][2].Change(_primaryColor);
			_bottomLights[i][3].Change(_secondaryColor);

			_topLights[i][0].Change(_secondaryColor);
			_topLights[i][1].Change(_primaryColor);
			_topLights[i][2].Change(_secondaryColor);
			_topLights[i][3].Change(_primaryColor);
		}
	}

	public void Checkered()
	{
		for (int i = 0; i < _bottomLights.Count; i++)
		{
			for (int j = 0; j < _bottomLights[i].Count; j++)
			{
				if (i % 2 == 0)
				{
					if (j % 2 == 0)
					{
						_bottomLights[i][j].Change(_primaryColor);
						_topLights[i][j].Change(_secondaryColor);
					}
					else
					{
						_bottomLights[i][j].Change(_secondaryColor);
						_topLights[i][j].Change(_primaryColor);
					}
				}
				else
				{
					if (j % 2 == 0)
					{
						_bottomLights[i][j].Change(_secondaryColor);
						_topLights[i][j].Change(_primaryColor);
					}
					else
					{
						_bottomLights[i][j].Change(_primaryColor);
						_topLights[i][j].Change(_secondaryColor);
					}
				}
			}
		}
	}

	public void Quads()
	{
		bool flip = true;
		for (int i = 0; i < _bottomLights.Count; i++)
		{
			if (flip)
			{
				_bottomLights[i][0].Change(_primaryColor);
				_bottomLights[i][1].Change(_primaryColor);
				_bottomLights[i][2].Change(_secondaryColor);
				_bottomLights[i][3].Change(_secondaryColor);
				_bottomLights[i][4].Change(_primaryColor);

				_topLights[i][0].Change(_secondaryColor);
				_topLights[i][1].Change(_secondaryColor);
				_topLights[i][2].Change(_primaryColor);
				_topLights[i][3].Change(_primaryColor);
				_topLights[i][4].Change(_secondaryColor);
			}
			else
			{
				_bottomLights[i][0].Change(_secondaryColor);
				_bottomLights[i][1].Change(_secondaryColor);
				_bottomLights[i][2].Change(_primaryColor);
				_bottomLights[i][3].Change(_primaryColor);
				_bottomLights[i][4].Change(_secondaryColor);

				_topLights[i][0].Change(_primaryColor);
				_topLights[i][1].Change(_primaryColor);
				_topLights[i][2].Change(_secondaryColor);
				_topLights[i][3].Change(_secondaryColor);
				_topLights[i][4].Change(_primaryColor);
			}

			if ((i + 1) % 3 == 0)
			{
				flip = !flip;
			}
		}
	}

	public void TheBeat()
	{
		for (int i = 0; i < _bottomLights.Count; i++)
		{
			for (int j = 0; j < _bottomLights[i].Count; j++)
			{
				if (_bottomLights[i][j].Equals(_primaryColor))
				{
					_bottomLights[i][j].Change(_secondaryColor);
					_topLights[i][j].Change(_primaryColor);
				}
				else if (_bottomLights[i][j].Equals(_secondaryColor))
				{
					_bottomLights[i][j].Change(_primaryColor);
					_topLights[i][j].Change(_secondaryColor);
				}
			}
		}
	}
}
