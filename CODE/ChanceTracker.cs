using Godot;
using System;

public partial class ChanceTracker : VBoxContainer
{
    public int _levelTracker;

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
        GetNode<Label>("LightingStyle").Text = String.Format("LightingStyle: {0}", (GetNode("../SubViewportContainer/SubViewport/Root/HallwayDisco") as HallwayDisco)._lightingStyle);

        GetNode<Sprite2D>("Sprite2D2").Position = new Vector2(GetNode<Sprite2D>("Sprite2D2").Position.X,
                                                                currentFocus.Position.Y + currentFocus.Size.Y * .5f);

        GetNode<Sprite2D>("Controls").Visible = GetNode<CheckButton>("CheckButton").ButtonPressed;

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

                //TODO: Fix switching between the levels and showing appropriate labels
                case "Level":
                    _levelTracker++;
                    _levelTracker %= 2;
                    var levels = Tools.GetChildren<Hallway>(GetNode("../SubViewportContainer/SubViewport/Root"));

                    foreach (var level in levels)
                    {
                        level.Visible = false;
                    }
                    levels[_levelTracker].Visible = true;
                    GetNode<Label>("Level").Text = String.Format("Level: {0}", levels[_levelTracker].Name);

                    if (levels[_levelTracker].Name == "HallwayDisco")
                    {
                        GetNode<Label>("Desk").Visible = false;
                        GetNode<Label>("Water").Visible = false;
                        GetNode<Label>("Light").Visible = false;
                        GetNode<Label>("Posters").Visible = false;
                        GetNode<Label>("Pace").Visible = false;

                        GetNode<Label>("LightingStyle").Visible = true;
                        //GetNode<Label>("Pace").Visible = true;
                    }

                    if (levels[_levelTracker].Name == "Hallway")
                    {
                        GetNode<Label>("Desk").Visible = true;
                        GetNode<Label>("Water").Visible = true;
                        GetNode<Label>("Light").Visible = true;
                        GetNode<Label>("Posters").Visible = true;
                        GetNode<Label>("Pace").Visible = true;

                        GetNode<Label>("LightingStyle").Visible = false;
                        //GetNode<Label>("Pace").Visible = true;
                    }

                    break;
            }
        }
        else if (Input.IsActionJustPressed("Decrease"))
        {
            switch (currentFocus.Name.ToString())
            {
                case "Pace":
                    Hallway._pace -= Hallway._pace >= 0f ? .05f : 0;
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
        //TODO: FIX Swapping control view
        if ((Input.IsActionJustPressed("Increase") || Input.IsActionJustPressed("Decrease")) && currentFocus == GetNode("CheckButton"))
        {
            (currentFocus as CheckButton).ButtonPressed = !(currentFocus as CheckButton).ButtonPressed;
        }
    }
}