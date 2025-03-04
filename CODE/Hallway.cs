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
        pace = .1f;
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

        if (Input.IsActionJustPressed("IncreasePace") && pace <= .5f)
            pace += .05f;

        if (Input.IsActionJustPressed("DecreasePace") && pace >= .1f)
            pace -= .05f;

        if (Input.IsActionJustPressed("IncreaseDesk") && HallwayPiece._deskChance < 100)
            HallwayPiece._deskChance += 10;

        if (Input.IsActionJustPressed("DecreaseDesk") && HallwayPiece._deskChance > 0)
            HallwayPiece._deskChance -= 10;

        if (Input.IsActionJustPressed("IncreaseWater") && HallwayPiece._waterCooler < 100)
            HallwayPiece._waterCooler += 10;

        if (Input.IsActionJustPressed("DecreaseWater") && HallwayPiece._waterCooler > 0)
            HallwayPiece._waterCooler -= 10;

        if (Input.IsActionJustPressed("IncreaseLightFlicker") && HallwayPiece._lightFlicker < 100)
            HallwayPiece._lightFlicker += 10;

        if (Input.IsActionJustPressed("DecreaseLightFlicker") && HallwayPiece._lightFlicker > 0)
            HallwayPiece._lightFlicker -= 10;

        if (Input.IsActionJustPressed("IncreasePosters") && HallwayPiece._posterChance < 100)
            HallwayPiece._posterChance += 10;

        if (Input.IsActionJustPressed("DecreasePosters") && HallwayPiece._posterChance > 0)
            HallwayPiece._posterChance -= 10;
    }
}