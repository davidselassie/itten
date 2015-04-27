using UnityEngine;

public enum GelColor : byte {
	White = 0x0,
	Cyan = 0x1,
	Magenta = 0x2,
	Yellow = 0x4,
	Red = 0x6,
	Green = 0x5,
	Blue = 0x3,
	Black = 0x7
}

public static class GelColorExtensions {
	public static GelColor Mix (this GelColor one, GelColor two) {
		return one | two;
	}
	
	public static GelColor Invert (this GelColor color) {
		return ~color;
	}
	
	public static Color GetRenderColor (this GelColor color) {
		return new Color(
			(color & GelColor.Cyan) == 0 ? 1.0f : 0.0f,
			(color & GelColor.Magenta) == 0 ? 1.0f : 0.0f,
			(color & GelColor.Yellow) == 0 ? 1.0f : 0.0f,
			0.5f);
	}

	public static GelColor FromRenderColor (Color renderColor) {
		float c = 1.0f - renderColor.r, m = 1.0f - renderColor.g, y = 1.0f - renderColor.b;
		return ((c >= 0.5f ? GelColor.Cyan : GelColor.White) |
		        (m >= 0.5f ? GelColor.Magenta : GelColor.White) |
		        (y >= 0.5f ? GelColor.Yellow : GelColor.White));
	}
}
