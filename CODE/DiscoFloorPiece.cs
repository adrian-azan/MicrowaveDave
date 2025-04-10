using Godot;
using Godot.Collections;

using System;

public partial class DiscoFloorPiece : Node3D
{
    public Light3D _light;
    public MeshInstance3D _mesh;

    private static Dictionary<string, string> _colors;

    public enum COLOR
    {
        RED, GREEN, BLUE
    }

    private COLOR currentColor;

    public override void _Ready()
    {
        currentColor = COLOR.BLUE;
        _mesh = GetNode<MeshInstance3D>("Floor");
        _light = GetNode<Light3D>("Light");
    }

    public bool Equals(COLOR color)
    {
        return currentColor == color;
    }

    public void Change(COLOR color)
    {
        ShaderMaterial coloredMesh = null;
        currentColor = color;
        switch (color)
        {
            case COLOR.RED:
                coloredMesh = ResourceLoader.Load<ShaderMaterial>("res://ART/MATERIALS AND MESHES/neonRed.tres");
                break;

            case COLOR.BLUE:
                coloredMesh = ResourceLoader.Load<ShaderMaterial>("res://ART/MATERIALS AND MESHES/neonBlue.tres");
                break;
        }

        _mesh.SetSurfaceOverrideMaterial(0, coloredMesh);
    }
}