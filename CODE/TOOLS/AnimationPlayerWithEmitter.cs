using Godot;
using System;

public partial class AnimationPlayerWithEmitter : AnimationPlayer
{

    //TODO: Need to be able to track what signals are being sent from where for diagramming
    public void EmitSignal(StringName signalName)
    {
        Logging.PrintInfo("AnimationPlayer",string.Format("{0} Emitting Signal {1}",GetParent().Name, signalName));
        CustomSignals._Instance.EmitSignal(signalName);
    }
    
}
