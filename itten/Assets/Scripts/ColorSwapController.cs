using UnityEngine;
using System.Collections;

[RequireComponent(typeof(ColorBehavior))]
public class ColorSwapController : MonoBehaviour {
	private ColorBehavior colorBehavior;

	void Start () {
		colorBehavior = GetComponent<ColorBehavior>();
	}

	void FixedUpdate () {
		if (Input.GetKeyDown ("m")) {
			Swap ();
		}
	}

	private GelColor Next (GelColor current) {
		switch (current) {
		case GelColor.Cyan:
			return GelColor.Magenta;
		case GelColor.Magenta:
			return GelColor.Yellow;
		case GelColor.Yellow:
			return GelColor.Cyan;
		case GelColor.Red:
			return GelColor.Green;
		case GelColor.Green:
			return GelColor.Blue;
		case GelColor.Blue:
			return GelColor.Red;
		case GelColor.Black:
			return GelColor.Black;
		default:
			return GelColor.White;
		}
	}

	void Swap () {
		colorBehavior.Color = Next (colorBehavior.Color);
		colorBehavior.ReckonColorChange ();
	}
}
