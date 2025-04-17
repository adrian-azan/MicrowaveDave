using Godot;
using System;

public partial class CustomSignals : Node
{
    [Signal]
    public delegate void UpdateLightsSignalEventHandler();

    [Signal]
    public delegate void UpdateShowTopSignalEventHandler(bool visible);
}