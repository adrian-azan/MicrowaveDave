using Godot;
using Godot.Collections;
using System.Linq;

public partial class Lanes : Node2D
{
    public enum LANES
    {
        LEFT = 0,
        MIDDLE = 1,
        RIGHT = 2
    }

    protected Array<Path2D> _lanes;

    public override void _Ready()
    {
        _lanes = new Array<Path2D>(Tools.GetChildren<Path2D>(this)
            .Where(node => node.Name != "Core")
            .ToArray());
        
        CustomSignals._Instance.HideLanes += HideLanes;
    }

    public Array<Node> EnemiesInLane(LANES lane)
    {
        return _lanes[((int)lane)].GetChildren();
    }

    public void AddAttackToLane(Attack attack, LANES lane)
    {
        _lanes[(int)lane].AddChild(attack);
    }

    public void ClearLanes()
    {
        foreach (var lane in _lanes)
        {
            foreach (Attack attack in lane.GetChildren())
            {
                attack.QueueFree();
            }
        }
    }

    public void ClearLanes(Node attacker)
    {
        foreach (var lane in _lanes)
        {
            foreach (Attack attack in lane.GetChildren())
            {
                if (attack.Attacker == attacker)
                {
                    attack.QueueFree();
                }
            }
        }
    }

    public void SetLaneCurve(LANES lane, Curve2D curve)
    {
        _lanes[(int)lane].Curve = curve;
    }

    public int Count()
    {
        return _lanes == null ? 0 : _lanes.Count;
    }

    public void HideLanes()
    {
        var hideableChildren = Tools.GetChildren<Sprite2D>(this);

        foreach (var child in hideableChildren)
        {
            Color currentColor = child.SelfModulate;
            currentColor.A = 0;
            CreateTween().TweenProperty(child, "self_modulate", currentColor, .1f);
        }
    }
}