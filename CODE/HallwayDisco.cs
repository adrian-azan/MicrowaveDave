using Godot;
using Godot.Collections;
using System.Linq;

public partial class HallwayDisco : Hallway
{
    private Array<PathFollow3D> _pieces;

    private Array<Array<DiscoFloorPiece>> _bottomLights;
    private Array<Array<DiscoFloorPiece>> _topLights;

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

    public static LightingStyle _lightingStyle;

    public override void _Ready()
    {
        base._Ready();

        _theBeat = GetNode<Timer>("Timer");
        _theBeat.Start();

        _pieces = Tools.GetChildren<PathFollow3D>(this);
        _bottomLights = new Array<Array<DiscoFloorPiece>>();
        _topLights = new Array<Array<DiscoFloorPiece>>();

        foreach (Node3D piece in _pieces)
        {
            Array<DiscoFloorPiece> lights = Tools.GetChildren<DiscoFloorPiece>(piece.GetNode("FloorTiles"));

            _bottomLights.Add(lights.Slice(0, 5));
            _bottomLights.Add(lights.Slice(5, 5));
            _bottomLights.Add(lights.Slice(10, 5));

            _topLights.Add(lights.Slice(15, 5));
            _topLights.Add(lights.Slice(20, 5));
            _topLights.Add(lights.Slice(25, 5));
        }

        GetNode<CustomSignals>("/root/CustomSignals").UpdateLightsSignal += UpdateLights;
    }

    public void UpdateLights()
    {
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
        for (int i = 0; i < _bottomLights.Count; i++)
        {
            _bottomLights[i][0].Change(_primaryColor);
            _bottomLights[i][1].Change(_primaryColor);
            _bottomLights[i][2].Change(_secondaryColor);
            _bottomLights[i][3].Change(_secondaryColor);

            _topLights[i][3].Change(_primaryColor);
            _topLights[i][2].Change(_primaryColor);
            _topLights[i][1].Change(_secondaryColor);
            _topLights[i][0].Change(_secondaryColor);
        }
    }

    public void FourLanes()
    {
        for (int i = 0; i < _bottomLights.Count; i++)
        {
            _bottomLights[i][0].Change(_primaryColor);
            _bottomLights[i][1].Change(_secondaryColor);
            _bottomLights[i][2].Change(_primaryColor);
            _bottomLights[i][3].Change(_secondaryColor);

            _topLights[i][0].Change(_secondaryColor);
            _topLights[i][1].Change(_primaryColor);
            _topLights[i][2].Change(_secondaryColor);
            _topLights[i][3].Change(_primaryColor);
        }
    }

    public void Checkered()
    {
        for (int i = 0; i < _bottomLights.Count; i++)
        {
            for (int j = 0; j < _bottomLights[i].Count; j++)
            {
                if (i % 2 == 0)
                {
                    if (j % 2 == 0)
                    {
                        _bottomLights[i][j].Change(_primaryColor);
                        _topLights[i][j].Change(_secondaryColor);
                    }
                    else
                    {
                        _bottomLights[i][j].Change(_secondaryColor);
                        _topLights[i][j].Change(_primaryColor);
                    }
                }
                else
                {
                    if (j % 2 == 0)
                    {
                        _bottomLights[i][j].Change(_secondaryColor);
                        _topLights[i][j].Change(_primaryColor);
                    }
                    else
                    {
                        _bottomLights[i][j].Change(_primaryColor);
                        _topLights[i][j].Change(_secondaryColor);
                    }
                }
            }
        }
    }

    public void Quads()
    {
        bool flip = true;
        for (int i = 0; i < _bottomLights.Count; i++)
        {
            if (flip)
            {
                _bottomLights[i][0].Change(_primaryColor);
                _bottomLights[i][1].Change(_primaryColor);
                _bottomLights[i][2].Change(_secondaryColor);
                _bottomLights[i][3].Change(_secondaryColor);
                _bottomLights[i][4].Change(_primaryColor);

                _topLights[i][0].Change(_secondaryColor);
                _topLights[i][1].Change(_secondaryColor);
                _topLights[i][2].Change(_primaryColor);
                _topLights[i][3].Change(_primaryColor);
                _topLights[i][4].Change(_secondaryColor);
            }
            else
            {
                _bottomLights[i][0].Change(_secondaryColor);
                _bottomLights[i][1].Change(_secondaryColor);
                _bottomLights[i][2].Change(_primaryColor);
                _bottomLights[i][3].Change(_primaryColor);
                _bottomLights[i][4].Change(_secondaryColor);

                _topLights[i][0].Change(_primaryColor);
                _topLights[i][1].Change(_primaryColor);
                _topLights[i][2].Change(_secondaryColor);
                _topLights[i][3].Change(_secondaryColor);
                _topLights[i][4].Change(_primaryColor);
            }

            if ((i + 1) % 3 == 0)
            {
                flip = !flip;
            }
        }
    }

    public void TheBeat()
    {
        for (int i = 0; i < _bottomLights.Count; i++)
        {
            for (int j = 0; j < _bottomLights[i].Count; j++)
            {
                if (_bottomLights[i][j].Equals(_primaryColor))
                {
                    _bottomLights[i][j].Change(_secondaryColor);
                    _topLights[i][j].Change(_primaryColor);
                }
                else if (_bottomLights[i][j].Equals(_secondaryColor))
                {
                    _bottomLights[i][j].Change(_primaryColor);
                    _topLights[i][j].Change(_secondaryColor);
                }
            }
        }
    }
}