using Godot;
using System;

public partial class HallwayDiscoPiece : PathFollow3D
{
    public override void _Ready()
    {
        
    }

    public void SetPiece()
    {
        var possiblePieces = Tools.GetChildren<NeonLightPanel>(this);
        for (int i = 0; i < possiblePieces.Count * (HallwayDisco._discoLightFlickerChance / 100); i++)
        {
            NeonLightPanel discoLight = possiblePieces.PickRandom();
            possiblePieces.Remove(discoLight);
            discoLight._flickering = true;
            discoLight.Flicker();
        }

        foreach (var remainingPiece in possiblePieces)
        {
            remainingPiece._flickering = false;
        }
    }
}