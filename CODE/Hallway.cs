using Godot;
using Godot.Collections;

public partial class Hallway : Node3D
{
    [Export]
    public PackedScene piece;

    public Array<PathFollow3D> pieces;

    public float pace;

    public override void _Ready()
    {
        pace = .01f;
        float totalPieces = 24;
        pieces = new Array<PathFollow3D>();
        for (int i = 0; i < totalPieces; i++)
        {
            pieces.Add(piece.Instantiate() as PathFollow3D);
            GetNode("Path3D").AddChild(pieces[i]);
            pieces[i].ProgressRatio = (float)i / totalPieces;
        }
    }

    public override void _Process(double delta)
    {
        float felta = (float)delta;
        foreach (PathFollow3D piece in pieces)
        {
            piece.ProgressRatio += felta * pace;
        }
    }
}