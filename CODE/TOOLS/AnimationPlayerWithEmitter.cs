using Godot;
using System;

public partial class AnimationPlayerWithEmitter : AnimationPlayer
{

    public void EmitSignal(StringName signalName)
    {
        Logging.PrintTemp(string.Format("{0} Emitting Signal {1}",GetParent().Name, signalName));
        CustomSignals._Instance.EmitSignal(signalName);
    }
    
}
