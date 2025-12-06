using Godot;
using Godot.Collections;
using System;
using System.Linq;
using System.Linq.Expressions;

public partial class Hallway : Node3D
{
    [Export]
    private PackedScene piecePackedScene;

    private Array<PathFollow3D> _pieces;
    
    public Array<Path3D> _lanes;

    [Export]
    public float _pace;
    [Export]
    public float _deskChance;
    [Export]
    public float _waterCoolerChance;
    [Export]
    public float _lightFlickerChance;
    [Export]
    public float _posterChance;

    
    public static float _enemyPace;

    [Export]
    public int _DEBUG_enemyPaceMultiplyer = 1;
    
    public override void _Ready()
    {
        _pace = .1f;
        float totalPieces = 24;
        _pieces = new Array<PathFollow3D>();
        for (int i = 0; i < totalPieces; i++)
        {
            _pieces.Add(piecePackedScene.Instantiate() as PathFollow3D);
            GetNode("Path3D").AddChild(_pieces[i]);
            _pieces[i].ProgressRatio = (float)i / totalPieces;
        }

        _lanes = new Array<Path3D>();
        _lanes.Add(GetNode<Path3D>("LeftLane"));
        _lanes.Add(GetNode<Path3D>("MiddleLane"));
        _lanes.Add(GetNode<Path3D>("RightLane"));
    }

    public override void _Process(double delta)
    {
        float felta = (float)delta;
        foreach (PathFollow3D piece in _pieces)
        {
            piece.ProgressRatio += felta * _pace;
        }
        
        foreach (HallwayPiece piece in _pieces)
        {
            piece._deskChance = _deskChance;
            piece._lightFlickerChance = _lightFlickerChance;
            piece._posterChance = _posterChance;
            piece._waterCoolerChance = _waterCoolerChance;
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

    public bool IsEmpty()
    {
        Array<Enemy> enemiesInLanes = new Array<Enemy>();
        enemiesInLanes.Add(_lanes[1].GetChildCount() > 0 ? _lanes[1].GetChild<Enemy>(0) : null);
        enemiesInLanes.Add(_lanes[0].GetChildCount() > 0 ? _lanes[0].GetChild<Enemy>(0) : null);
        enemiesInLanes.Add(_lanes[2].GetChildCount() > 0 ? _lanes[2].GetChild<Enemy>(0) : null);

        enemiesInLanes = new Array<Enemy>(enemiesInLanes.Where(enemy => enemy != null && !enemy.Dead()));

        return enemiesInLanes.Count == 0;
    }
    
    public void LightsOn()
    {
        foreach (HallwayPiece piece in _pieces)
        {
            piece.LightsOn();
        }    
    }
    public void LightsOff()
    {
        foreach (HallwayPiece piece in _pieces)
        {
            piece.LightsOff();
        }    
    }

    public void Clear()
    {
        _deskChance = 0;
        _waterCoolerChance = 0;
        _lightFlickerChance = 0;
        _posterChance = 0;
        foreach (HallwayPiece piece in _pieces)
        {
            piece._deskChance = _deskChance;
            piece._lightFlickerChance = _lightFlickerChance;
            piece._posterChance = _posterChance;
            piece._waterCoolerChance = _waterCoolerChance;
            piece.SetPiece();
        }   
    }
}