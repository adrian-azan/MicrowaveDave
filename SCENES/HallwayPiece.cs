using Godot;
using Godot.Collections;
using System;

public partial class HallwayPiece : PathFollow3D
{
    public static int _deskChance;
    public static int _waterCooler;
    public static int _lightFlicker;
    public static int _posterChance;

    private Array<MeshInstance3D> _posters;

    public override void _Ready()
    {
        _deskChance = 0;
        _waterCooler = 0;
        _lightFlicker = 0;
        _posterChance = 0;
        _posters = new Array<MeshInstance3D>();

        _posters = Tools.GetChildren<MeshInstance3D>(GetNode("Posters"));

        SetPiece();
    }

    public void SetPiece()
    {
        RandomNumberGenerator rng = new RandomNumberGenerator();
        GetNode<Node3D>("Desk").Visible = rng.RandiRange(0, 100) < _deskChance ? true : false;
        GetNode<Node3D>("WaterCooler").Visible = rng.RandiRange(0, 100) < _waterCooler ? true : false;

        foreach (var poster in _posters)
        {
            poster.Visible = rng.RandiRange(0, 100) < _posterChance ? true : false;
            poster.RotateX(Mathf.DegToRad(rng.RandfRange(-20, 20)));
        }
        if (rng.RandiRange(0, 100) < _lightFlicker)
        {
            GetNode<AnimationPlayer>("AnimationPlayer").Play("Flicker");
            GetNode<AnimationPlayer>("AnimationPlayer").SpeedScale = rng.RandfRange(.25f, 1);
        }
        else
        {
            GetNode<AnimationPlayer>("AnimationPlayer").Stop();
        }
    }
}