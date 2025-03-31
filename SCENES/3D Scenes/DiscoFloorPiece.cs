using Godot;
using Godot.Collections;

using System;

public partial class DiscoFloorPiece : Node3D
{
    public OmniLight3D _light;
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
        _light = GetNode<OmniLight3D>("Light");
    }

    public bool Equals(COLOR color)
    {
        return currentColor == color;
    }

    public void Change(COLOR color)
    {
        string meshPath = "";
        currentColor = color;
        switch (color)
        {
            case COLOR.RED:
                meshPath = "res://SCENES/3D Scenes/RedFloorMesh.tres";
                break;

            case COLOR.BLUE:
                meshPath = "res://SCENES/3D Scenes/BlueFloorMesh.tres";
                break;
        }

        BoxMesh coloredMesh = ResourceLoader.Load<BoxMesh>(meshPath);

        if (coloredMesh == null)
            return;

        _light.LightColor = (coloredMesh.Material as StandardMaterial3D).AlbedoColor;
        _mesh.Mesh = coloredMesh;
    }
}