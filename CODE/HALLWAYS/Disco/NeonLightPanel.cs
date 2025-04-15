using Godot;
using Godot.Collections;

using System;

public partial class NeonLightPanel : Node3D
{
    public MeshInstance3D _mesh;
    public bool _flickering;

    private static Dictionary<string, string> _colors;

    public enum COLOR
    {
        RED, GREEN, BLUE, BLACK
    }

    private COLOR currentColor;

    public override void _Ready()
    {
        currentColor = COLOR.BLUE;
        _mesh = GetNode<MeshInstance3D>("Floor");
        _flickering = false;
    }

    public bool Equals(COLOR color)
    {
        return currentColor == color;
    }

    public void Change(COLOR color)
    {
        if (_flickering) return;

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

            case COLOR.BLACK:
                coloredMesh = ResourceLoader.Load<ShaderMaterial>("res://ART/MATERIALS AND MESHES/neonBlack.tres");
                break;
        }

        _mesh.SetSurfaceOverrideMaterial(0, coloredMesh);
    }

    public void Flicker()
    {
        ShaderMaterial coloredMesh = null;
        switch (currentColor)
        {
            case COLOR.RED:
                coloredMesh = ResourceLoader.Load<ShaderMaterial>("res://ART/MATERIALS AND MESHES/neonRed.tres");
                break;

            case COLOR.BLUE:
                coloredMesh = ResourceLoader.Load<ShaderMaterial>("res://ART/MATERIALS AND MESHES/neonBlue.tres");
                break;

            case COLOR.BLACK:
                coloredMesh = ResourceLoader.Load<ShaderMaterial>("res://ART/MATERIALS AND MESHES/neonBlack.tres");
                break;
        }

        var tween = CreateTween();
        tween.TweenCallback(Callable.From(() => GetNode<MeshInstance3D>("Floor").SetSurfaceOverrideMaterial(0, ResourceLoader.Load<Material>("res://ART/MATERIALS AND MESHES/neonBlack.tres"))));
        tween.TweenCallback(Callable.From(() => GetNode<MeshInstance3D>("Floor").SetSurfaceOverrideMaterial(0, coloredMesh))).SetDelay(.1f);
        tween.TweenCallback(Callable.From(() => GetNode<MeshInstance3D>("Floor").SetSurfaceOverrideMaterial(0, ResourceLoader.Load<Material>("res://ART/MATERIALS AND MESHES/neonBlack.tres")))).SetDelay(.2f);
        tween.TweenCallback(Callable.From(() => GetNode<MeshInstance3D>("Floor").SetSurfaceOverrideMaterial(0, coloredMesh))).SetDelay(.3f);
        tween.TweenCallback(Callable.From(() => GetNode<MeshInstance3D>("Floor").SetSurfaceOverrideMaterial(0, ResourceLoader.Load<Material>("res://ART/MATERIALS AND MESHES/neonBlack.tres")))).SetDelay(.6f);
        if (_flickering)
            tween.TweenCallback(Callable.From(() => Flicker())).SetDelay(.8f);
    }
}