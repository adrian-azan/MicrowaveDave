using Godot;
using Godot.Collections;
using System;
using static CustomSignals;

public partial class ChanceTracker : VBoxContainer
{
    public int _levelTracker;

    public Dictionary<string, Array<Control>> _options;

    public override void _Ready()
    {
        GetNode<Label>("Pace").GrabFocus();

        _options = new Dictionary<string, Array<Control>>();

        _options.Add("Level1", new Array<Control>());
        _options.Add("Level2", new Array<Control>());

        _options["Level1"].Add(GetNode<Label>("Desk"));
        _options["Level1"].Add(GetNode<Label>("Water"));
        _options["Level1"].Add(GetNode<Label>("Light"));
        _options["Level1"].Add(GetNode<Label>("Posters"));

        _options["Level2"].Add(GetNode<Label>("LightingStyle"));
        _options["Level2"].Add(GetNode<CheckButton>("ShowTop"));

        SwitchLevel();
    }

    public override void _Process(double delta)
    {
        Control currentFocus = GetViewport().GuiGetFocusOwner();

        GetNode<Label>("Desk").Text = String.Format("Desks: {0}%", HallwayPiece._deskChance);
        GetNode<Label>("Water").Text = String.Format("Water: {0}%", HallwayPiece._waterCoolerChance);
        GetNode<Label>("Light").Text = String.Format("Light: {0}%", HallwayPiece._lightFlickerChance);
        GetNode<Label>("Posters").Text = String.Format("Poster: {0}%", Math.Round((double)HallwayPiece._posterChance, 0));
        GetNode<Label>("Pace").Text = String.Format("Pace: {0}x", Math.Round(Hallway._pace * 10, 1));
        GetNode<Label>("LightingStyle").Text = String.Format("LightingStyle: {0}", HallwayDisco._lightingStyle);

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

                case "LightingStyle":
                    HallwayDisco._lightingStyle++;
                    HallwayDisco._lightingStyle = (HallwayDisco.LightingStyle)(((int)HallwayDisco._lightingStyle) % 4);
                    GetNode("/root/CustomSignals").EmitSignal(CustomSignals.SignalName.UpdateLightsSignal);

                    break;

                case "ShowTop":
                    CheckButton checkButton = GetNode<CheckButton>("ShowTop");
                    checkButton.ButtonPressed = !checkButton.ButtonPressed;
                    GetNode("/root/CustomSignals").EmitSignal(CustomSignals.SignalName.UpdateShowTopSignal, checkButton.ButtonPressed);
                    break;

                //TODO: Fix switching between the levels and showing appropriate labels
                case "Level":
                    SwitchLevel();
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

                case "LightingStyle":
                    HallwayDisco._lightingStyle--;
                    HallwayDisco._lightingStyle = HallwayDisco._lightingStyle < 0 ? (HallwayDisco.LightingStyle)3 : (HallwayDisco.LightingStyle)((int)HallwayDisco._lightingStyle);
                    GetNode("/root/CustomSignals").EmitSignal(CustomSignals.SignalName.UpdateLightsSignal);
                    break;
            }
        }
        //TODO: FIX Swapping control view
        if ((Input.IsActionJustPressed("Increase") || Input.IsActionJustPressed("Decrease")) && currentFocus == GetNode("CheckButton"))
        {
            (currentFocus as CheckButton).ButtonPressed = !(currentFocus as CheckButton).ButtonPressed;
        }
    }

    public void SwitchLevel()
    {
        _levelTracker++;
        _levelTracker %= 2;
        var levels = Tools.GetChildren<Hallway>(GetNode("../SubViewportContainer/SubViewport/Root"));

        foreach (var level in levels)
        {
            level.Visible = false;
        }
        levels[_levelTracker].Visible = true;

        if (levels[_levelTracker].Name == "HallwayDisco")
        {
            foreach (Control property in _options["Level1"])
            {
                property.Visible = false;
            }

            foreach (Control property in _options["Level2"])
            {
                property.Visible = true;
            }

            Label pace = GetNode<Label>("Pace");
            Label level = GetNode<Label>("Level");

            pace.FocusNeighborBottom = _options["Level2"][0].GetPath();
            level.FocusNeighborTop = _options["Level2"][_options["Level2"].Count - 1].GetPath();

            _options["Level2"][0].FocusNeighborTop = pace.GetPath();
            _options["Level2"][_options["Level2"].Count - 1].FocusNeighborBottom = level.GetPath();
        }

        if (levels[_levelTracker].Name == "Hallway")
        {
            foreach (Control property in _options["Level1"])
            {
                property.Visible = true;
            }

            foreach (Control property in _options["Level2"])
            {
                property.Visible = false;
            }

            Label pace = GetNode<Label>("Pace");
            Label level = GetNode<Label>("Level");

            pace.FocusNeighborBottom = _options["Level1"][0].GetPath();
            level.FocusNeighborTop = _options["Level1"][_options["Level1"].Count - 1].GetPath();

            _options["Level1"][0].FocusNeighborTop = pace.GetPath();
            _options["Level1"][_options["Level1"].Count - 1].FocusNeighborBottom = level.GetPath();
        }
    }
}