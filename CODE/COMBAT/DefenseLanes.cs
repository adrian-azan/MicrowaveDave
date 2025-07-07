using Godot;

public partial class DefenseLanes : Lanes
{
    public int _stamina;

    public override void _Ready()
    {
        base._Ready();
        _stamina = 3;

        CustomSignals._Instance.RecoverStaminaSignal += RecoverStamina;

        GetNode<Area2D>("Core/PlayerDefense/Area2D").AreaEntered += DefensiveStyle;
        GetNode<Area2D>("Core/PlayerDefense/Area2D").AreaExited += DefaultStyle;

        GetNode<Area2D>("Core/PlayerCounter/Area2D").AreaEntered += CounterStyle;
        GetNode<Area2D>("Core/PlayerCounter/Area2D").AreaExited += DefaultStyle;
    }

    public override void _Process(double delta)
    {
        float felta = (float)delta;

        GetNode<Label>("Label").Text = $"Stamina: {_stamina}";

        if (Input.IsActionJustPressed("LeftAttack") && _stamina > 0)
        {
            var attackScene = ResourceLoader.Load<PackedScene>(Constants.ATTACK_SCENE);
            var attackInstance = attackScene.Instantiate<Attack>();

            attackInstance._pace = .4f;
            attackInstance.GetNode<Area2D>("Area2D").SetCollisionLayerValue(6, true);
            attackInstance.GetNode<Area2D>("Area2D").SetCollisionLayerValue(2, false);

            AddAttackToLane(attackInstance, LANES.LEFT);

            _stamina -= 1;
        }

        if (Input.IsActionJustPressed("MiddleAttack") && _stamina > 0)
        {
            var attackScene = ResourceLoader.Load<PackedScene>(Constants.ATTACK_SCENE);
            var attackInstance = attackScene.Instantiate<Attack>();

            attackInstance._pace = .4f;
            attackInstance.GetNode<Area2D>("Area2D").SetCollisionLayerValue(6, true);
            attackInstance.GetNode<Area2D>("Area2D").SetCollisionLayerValue(2, false);

            AddAttackToLane(attackInstance, LANES.MIDDLE);

            _stamina -= 1;
        }

        if (Input.IsActionJustPressed("RightAttack") && _stamina > 0)
        {
            var attackScene = ResourceLoader.Load<PackedScene>(Constants.ATTACK_SCENE);
            var attackInstance = attackScene.Instantiate<Attack>();

            attackInstance._pace = .4f;
            attackInstance.GetNode<Area2D>("Area2D").SetCollisionLayerValue(6, true);
            attackInstance.GetNode<Area2D>("Area2D").SetCollisionLayerValue(2, false);

            AddAttackToLane(attackInstance, LANES.RIGHT);

            _stamina -= 1;
        }
    }

    public void RecoverStamina()
    {
        if (_stamina < 3)
            _stamina += 1;
    }

    public void DefaultStyle(Area2D area)
    {
        (area.GetParent() as Attack)._style = Attack.Style.OFFENSIVE;
    }

    public void DefensiveStyle(Area2D area)
    {
        (area.GetParent() as Attack)._style = Attack.Style.DEFENSIVE;
    }

    public void CounterStyle(Area2D area)
    {
        (area.GetParent() as Attack)._style = Attack.Style.COUNTERING;
    }

    public void CollisionWithHeart(Area2D incomingAttack)
    {
        incomingAttack.GetParent().QueueFree();
    }
}