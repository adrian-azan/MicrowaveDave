using Godot;
using Godot.Collections;
public partial class Digit : Node3D
{
	/*
	 * The key represents the symbol that will display.
	 * For letters, the Ascii value is the key. (48 = A, 49 = B, etc).
	 * The value is an array of which parts of the display are on or off.
	 * The index number below represents the lights that will display if the
	 * value at that index is true.
	 *
	 * - 0 -
	 * |   |
	 * 5   1
	 * |-6-|
	 * 4   2
	 * |   |
	 * - 3 -
	 */
	Array<Light3D> _lights;
	private const int LIGHT_ENERGY = 5;

	private static Dictionary<int, Array<bool>> digitToLight = new Dictionary<int, Array<bool>> {
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
	}

	public void SetDigit(int value)
	{
		var activeTicks = digitToLight[value];

		for (int i = 0; i < activeTicks.Count; i++)
		{
			_lights[i].LightEnergy = activeTicks[i] ? LIGHT_ENERGY : 0;
		}
	}
}
