using Godot;

public partial class DefenseLanesWithCounter : DefenseLanes
{
    public override void _Ready()
    {
        base._Ready();

        GetNode<Area2D>("Core/PlayerCounter/Area2D").AreaEntered += CounterStyle;
        GetNode<Area2D>("Core/PlayerCounter/Area2D").AreaExited += DefaultStyle;
    }

    public void CounterStyle(Area2D area)
    {
        (area.GetParent() as Attack)._style = Attack.Style.COUNTERING;
    }
}