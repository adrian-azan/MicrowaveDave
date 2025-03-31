using Godot;
using Godot.Collections;
using System.Linq;

public partial class HallwayDisco : Hallway
{
    private Array<PathFollow3D> _pieces;

    private Array<Array<DiscoFloorPiece>> _lights;

    public Timer _theBeat;

    [Export]
    public DiscoFloorPiece.COLOR _primaryColor;

    [Export]
    public DiscoFloorPiece.COLOR _secondaryColor;

    public enum LightingStyle
    {
        QUADS,
        CHECKERED,
        TWOLANES,
        FOURLANES
    };

    [Export]
    public LightingStyle _lightingStyle;

    public override void _Ready()
    {
        base._Ready();

        _theBeat = GetNode<Timer>("Timer");
        _theBeat.Start();

        _pieces = Tools.GetChildren<PathFollow3D>(this);
        _lights = new Array<Array<DiscoFloorPiece>>();

        foreach (Node3D piece in _pieces)
        {
            Array<DiscoFloorPiece> lights = Tools.GetChildren<DiscoFloorPiece>(piece.GetNode("FloorTiles"));

            _lights.Add(lights.Slice(0, 5));
            _lights.Add(lights.Slice(5, 5));
            _lights.Add(lights.Slice(10, 5));
        }

        switch (_lightingStyle)
        {
            case LightingStyle.QUADS:
                Quads();
                break;

            case LightingStyle.CHECKERED:
                Checkered();
                break;

            case LightingStyle.FOURLANES:
                FourLanes();
                break;

            case LightingStyle.TWOLANES:
                TwoLanes();
                break;
        }
    }

    public void ChangeColors(DiscoFloorPiece.COLOR primary, DiscoFloorPiece.COLOR secondary)
    {
    }

    public void TwoLanes()
    {
        for (int i = 0; i < _lights.Count; i++)
        {
            _lights[i][0].Change(_primaryColor);
            _lights[i][1].Change(_primaryColor);
            _lights[i][2].Change(_secondaryColor);
            _lights[i][3].Change(_secondaryColor);
        }
    }

    public void FourLanes()
    {
        for (int i = 0; i < _lights.Count; i++)
        {
            _lights[i][0].Change(_primaryColor);
            _lights[i][1].Change(_secondaryColor);
            _lights[i][2].Change(_primaryColor);
            _lights[i][3].Change(_secondaryColor);
        }
    }

    public void Checkered()
    {
        for (int i = 0; i < _lights.Count; i++)
        {
            for (int j = 0; j < _lights[i].Count; j++)
            {
                if (i % 2 == 0)
                {
                    if (j % 2 == 0)
                    {
                        _lights[i][j].Change(_primaryColor);
                    }
                    else
                    {
                        _lights[i][j].Change(_secondaryColor);
                    }
                }
                else
                {
                    if (j % 2 == 0)
                    {
                        _lights[i][j].Change(_secondaryColor);
                    }
                    else
                    {
                        _lights[i][j].Change(_primaryColor);
                    }
                }
            }
        }
    }

    public void Quads()
    {
        bool flip = true;
        for (int i = 0; i < _lights.Count; i++)
        {
            if (flip)
            {
                _lights[i][0].Change(_primaryColor);
                _lights[i][1].Change(_primaryColor);
                _lights[i][2].Change(_secondaryColor);
                _lights[i][3].Change(_secondaryColor);
                _lights[i][4].Change(_primaryColor);
            }
            else
            {
                _lights[i][0].Change(_secondaryColor);
                _lights[i][1].Change(_secondaryColor);
                _lights[i][2].Change(_primaryColor);
                _lights[i][3].Change(_primaryColor);
                _lights[i][4].Change(_secondaryColor);
            }

            if ((i + 1) % 3 == 0)
            {
                flip = !flip;
            }
        }
    }

    public void TheBeat()
    {
        for (int i = 0; i < _lights.Count; i++)
        {
            for (int j = 0; j < _lights[i].Count; j++)
            {
                if (_lights[i][j].Equals(_primaryColor))
                {
                    _lights[i][j].Change(_secondaryColor);
                }
                else if (_lights[i][j].Equals(_secondaryColor))
                {
                    _lights[i][j].Change(_primaryColor);
                }
            }
        }
    }
}