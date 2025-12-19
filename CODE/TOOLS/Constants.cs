using Godot;
using System;
using System.IO;
using System.Reflection;

public partial class Constants : Node
{
	public static NodePath ATTACK_LANES_FROM_ENEMY = new NodePath("../../../../../AttackLanes");
	public static NodePath DEFENSE_LANES_FROM_PLAYER = new NodePath("../../../../../DefenseLanes");

	public static String ATTACK_SCENE = "res://SCENES/Battle System/Jab Types/Attack.tscn";

	public static String TOTORIAL_ENEMY_1;
}
