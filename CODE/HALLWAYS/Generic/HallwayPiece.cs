using Godot;
using Godot.Collections;
using System;

public partial class HallwayPiece : PathFollow3D
{
    public static int _deskChance;
    public static int _waterCoolerChance;
    public static int _lightFlickerChance;
    public static int _posterChance;

    private Array<MeshInstance3D> _posters;
    private Node3D _desk;
    private Node3D _waterCooler;
    private AnimationPlayer _animationPlayer;

    public override void _Ready()
    {
        _deskChance = 0;
        _waterCoolerChance = 0;
        _lightFlickerChance = 0;
        _posterChance = 0;
        _posters = Tools.GetChildren<MeshInstance3D>(GetNode("Posters"));
        _desk = GetNode<Node3D>("Desk");
        _waterCooler = GetNode<Node3D>("WaterCooler");
        _animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");

        SetPiece();
    }

    public void SetPiece()
    {
        _desk.Visible = Tools.rng.RandiRange(0, 100) < _deskChance ? true : false;
        _waterCooler.Visible = Tools.rng.RandiRange(0, 100) < _waterCoolerChance ? true : false;

        foreach (var poster in _posters)
        {
            poster.Visible = Tools.rng.RandiRange(0, 100) < _posterChance ? true : false;
            poster.RotateX(Mathf.DegToRad(Tools.rng.RandfRange(-20, 20)));
        }

        if (Tools.rng.RandiRange(0, 100) < _lightFlickerChance)
        {
            _animationPlayer.Play("Flicker");
            _animationPlayer.SpeedScale = Tools.rng.RandfRange(.25f, 1);
        }
        else
        {
            _animationPlayer.Stop();
        }
    }
}