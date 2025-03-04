using Godot;
using System;

public partial class ChanceTracker : VBoxContainer
{
    public override void _Process(double delta)
    {
        GetNode<Label>("Desk").Text = String.Format("Desks: {0}%", HallwayPiece._deskChance);
        GetNode<Label>("Water").Text = String.Format("Water: {0}%", HallwayPiece._waterCoolerChance);
        GetNode<Label>("Light").Text = String.Format("Light: {0}%", HallwayPiece._lightFlickerChance);
        GetNode<Label>("Posters").Text = String.Format("Poster: {0}%", Math.Round((double)HallwayPiece._posterChance, 0));
        GetNode<Label>("Pace").Text = String.Format("Pace: {0}x speed", Math.Round(Hallway._pace * 10, 1));
    }
}