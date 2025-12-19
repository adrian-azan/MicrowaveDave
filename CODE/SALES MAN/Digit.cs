using Godot;
using Godot.Collections;
public partial class Digit : Node3D
{
	Array<Light3D> _lights;

	public static Dictionary<int, Array<bool>> digitToLight = new Dictionary<int, Array<bool>> {
		{1, new Array<bool>{false, true, true, false, false, false, false } },
		{2, new Array<bool>{true, true, false, true, true, false, true } },
		{3, new Array<bool>{true, true, true, true, false, false, true } },
		{4, new Array<bool>{false, true, true, false, false, true, true } },
		{5, new Array<bool>{true, false, true, true, false, true, true } },
		{6, new Array<bool>{true, false, true, true, true, true, true } },
		{7, new Array<bool>{true, true, true, false, false, false, false } },
		{8, new Array<bool>{true, true, true, true, true, true, true } },
		{9, new Array<bool>{true, true, true, false, false, true, true } },
		{0, new Array<bool>{true, true, true, true, true, true, false } },
		{48, new Array<bool>{true, true, true, false, true, true, true } },
		{50, new Array<bool>{true, false, false, true, true, true, false } }
	};

	public override void _Ready()
	{
		var ticks = GetChildren();
		_lights = Tools.GetChildren<Light3D>(this);

	   // var test = Tools.GetChildren<Light3D>(this);
		
		
		/*foreach (var light in ticks)
		{
			if (light.GetNode("OmniLight3D") != null)
				_lights.Add(light.GetNode("OmniLight3D") as Light3D);
		}*/
	}

	public void SetDigit(int value)
	{
		var activeTicks = digitToLight[value];

		for (int i = 0; i < 7; i++)
		{
			_lights[i].LightEnergy = activeTicks[i] ? 5 : 0;
		}
	}
}
