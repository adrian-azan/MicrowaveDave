using Godot;
using System;

public partial class ChanceTracker : VBoxContainer
{
    public override void _Process(double delta)
    {
        GetNode<Label>("Desk").Text = String.Format("Desks: {0}%", HallwayPiece._deskChance);
        GetNode<Label>("Water").Text = String.Format("Water: {0}%", HallwayPiece._waterCooler);
        GetNode<Label>("Light").Text = String.Format("Light: {0}%", HallwayPiece._lightFlicker);
        GetNode<Label>("Posters").Text = String.Format("Poster: {0}%", HallwayPiece._posterChance);
    }
}