using Godot;
using Godot.Collections;
using System;
using System.IO;
using System.Linq;

public partial class Attack : PathFollow2D
{
    public enum Style
    {
        OFFENSIVE, DEFENSIVE
    }

    public int _lane;
    public float health;
    public float attack;

    public float _pace;

    public bool countering;
    public Style _style;

    public Tween PROGRESS_TWEEN;
    public Tween PACE_TWEEN;

    [Export]
    private PackedScene _AttackScene;

    public override void _Ready()
    {
        health = 1.0f;
        attack = 0.34f;
        _pace = 1;
    }

    public void Set(float duration, int lane)
    {
        _lane = lane;
    }

    public override void _Process(double delta)
    {
        if (PROGRESS_TWEEN == null || !PROGRESS_TWEEN.IsValid())
        {
            ProgressRatio += (float)delta * _pace;
        }
    }

    public void CollisionWithIncomingJab(Area2D incomingAttack)
    {
        //if (countering)
        //{
        //    incomingAttack.GetParent().QueueFree();
        //    return;
        //}

        //Attack incomingJab = incomingAttack.GetParent<Attack>();

        //health -= incomingJab.attack;
        //incomingJab.health -= attack;

        //GetNode<Sprite2D>("Sprite").Scale = new Vector2(Mathf.Max(.25f, health), Mathf.Max(.25f, health));
        //incomingJab.GetNode<Sprite2D>("Sprite").Scale = new Vector2(Mathf.Max(.25f, incomingJab.health), Mathf.Max(.25f, incomingJab.health));

        ////   animationPlayer.SpeedScale = Scale.X;
        ////   incomingJab.animationPlayer.SpeedScale = incomingJab.Scale.X;

        //if (health <= 0)
        //    QueueFree();

        //if (incomingJab.health <= 0)
        //    incomingJab.QueueFree();
    }
}