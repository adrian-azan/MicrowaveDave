using Godot;
using System;

public partial class ChanceTracker : VBoxContainer
{
    public override void _Ready()
    {
        GetNode<Label>("Pace").GrabFocus();
    }

    public override void _Process(double delta)
    {
        Control currentFocus = GetViewport().GuiGetFocusOwner();

        GetNode<Label>("Desk").Text = String.Format("Desks: {0}%", HallwayPiece._deskChance);
        GetNode<Label>("Water").Text = String.Format("Water: {0}%", HallwayPiece._waterCoolerChance);
        GetNode<Label>("Light").Text = String.Format("Light: {0}%", HallwayPiece._lightFlickerChance);
        GetNode<Label>("Posters").Text = String.Format("Poster: {0}%", Math.Round((double)HallwayPiece._posterChance, 0));
        GetNode<Label>("Pace").Text = String.Format("Pace: {0}x", Math.Round(Hallway._pace * 10, 1));

        GetNode<Sprite2D>("Sprite2D2").Position = new Vector2(GetNode<Sprite2D>("Sprite2D2").Position.X,
                                                                currentFocus.Position.Y + currentFocus.Size.Y * .5f);

        if (Input.IsActionJustPressed("Increase"))
        {
            switch (currentFocus.Name.ToString())
            {
                case "Pace":
                    Hallway._pace += Hallway._pace < .5f ? .05f : 0;
                    break;

                case "Desk":
                    HallwayPiece._deskChance += HallwayPiece._deskChance < 100 ? 10 : 0;
                    break;

                case "Water":
                    HallwayPiece._waterCoolerChance += HallwayPiece._waterCoolerChance < 100 ? 10 : 0;
                    break;

                case "Light":
                    HallwayPiece._lightFlickerChance += HallwayPiece._lightFlickerChance < 100 ? 10 : 0;
                    break;

                case "Posters":
                    HallwayPiece._posterChance += HallwayPiece._posterChance < 100 ? 10 : 0;
                    break;
            }
        }
        else if (Input.IsActionJustPressed("Decrease"))
        {
            switch (currentFocus.Name.ToString())
            {
                case "Pace":
                    Hallway._pace -= Hallway._pace >= .1f ? .05f : 0;
                    break;

                case "Desk":
                    HallwayPiece._deskChance -= HallwayPiece._deskChance > 0 ? 10 : 0;
                    break;

                case "Water":
                    HallwayPiece._waterCoolerChance -= HallwayPiece._waterCoolerChance > 0 ? 10 : 0;
                    break;

                case "Light":
                    HallwayPiece._lightFlickerChance -= HallwayPiece._lightFlickerChance > 0 ? 10 : 0;
                    break;

                case "Posters":
                    HallwayPiece._posterChance -= HallwayPiece._posterChance > 0 ? 10 : 0;
                    break;
            }
        }
    }
}