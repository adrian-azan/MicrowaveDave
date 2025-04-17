using Godot;
using System;
using System.Linq;

public partial class Jab : PathFollow2D
{
    public float speed;
    public float health;
    public float attack;

    public AnimationPlayer animationPlayer;

    public override void _Ready()
    {
        speed = 1.0f;
        health = 1.0f;
        attack = 0.34f;

        animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
    }

    public void CollisionWithIncomingJab(Area2D incomingAttack)
    {
        if (ProgressRatio <= .3 && ProgressRatio >= .2)
        {
            incomingAttack.GetParent().QueueFree();
            return;
        }

        Jab incomingJab = incomingAttack.GetParent<Jab>();

        health -= incomingJab.attack;
        incomingJab.health -= attack;

        GetNode<Sprite2D>("Jab").Scale = new Vector2(Mathf.Max(.25f, health), Mathf.Max(.25f, health));
        incomingJab.GetNode<Sprite2D>("Jab").Scale = new Vector2(Mathf.Max(.25f, incomingJab.health), Mathf.Max(.25f, incomingJab.health));

        animationPlayer.SpeedScale = Scale.X;
        incomingJab.animationPlayer.SpeedScale = incomingJab.Scale.X;

        if (health <= 0)
            QueueFree();

        if (incomingJab.health <= 0)
            incomingJab.QueueFree();
    }
}