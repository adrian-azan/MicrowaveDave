using Godot;
using System;
using System.Collections.Generic;

public static class Logging
{

	//As I log more, more categories will likely come up.
	public class Categories
	{
		public static String FILE_MANAGEMENT = "File Management";
		public static String DATA_MANAGEMENT = "Data Management";
	}

	private static Dictionary<String, float> Timers = new Dictionary<string, float>();
	
	private static string TEMP_COLOR = "d5ff00";
	private static string INFO_COLOR = "4285f4";
	private static string WARNING_COLOR = "d88e00";
	private static string ERROR_COLOR = "da2c38";
	
	private static string TEMP_SIZE = "12";
	private static string INFO_SIZE = "12";
	private static string WARNING_SIZE = "150";
	private static string ERROR_SIZE = "150";

	
	public static void PrintTemp(String message)
	{
		GD.PrintRich($"[color=#{TEMP_COLOR}][font_size={TEMP_SIZE}]{message}[/font_size][/color]");
	}
	
	/*
	 * Timer Key is to help track duration between logs
	 * Example:
	 * PrintInfo("Enemies", "Loading start", "Load time")
	   ...
	   PrintInfo("Enemies", "Loading end", "Load time")
	 */
	public static void PrintInfo(String category, String message, String timerKey = null)
	{
		try
		{
			if (timerKey != null)
			{
				if (Timers.ContainsKey(timerKey))
				{
					message = $"{message,-20} {(Time.GetTicksMsec() - Timers[timerKey]) / 1000f}";
					Timers.Remove(timerKey);
				}
				else
				{
					Timers[timerKey] = Time.GetTicksMsec();
				}
			}
		}
		catch (Exception e)
		{
			GD.PrintErr(e.ToString());
		}

		GD.PrintRich($"[color=#{INFO_COLOR}][font_size={INFO_SIZE}]{category, -20}[/font_size][/color] {message}");
	}
	
	public static void PrintWarning(String category, String message)
	{
		GD.PrintRich($"[color=#{WARNING_COLOR}][size={WARNING_SIZE}]{category,-20}[/size][/color] {message}");
	}
	
	public static void PrintError(String category, String message)
	{
		GD.PrintRich($"[color=#{ERROR_COLOR}][size={ERROR_SIZE}]{category,20}[/color][/size=] {message}");
	}
}
