using UnityEngine;
using System.Collections;

[RequireComponent(typeof(ColorBehavior))]
public class ColorCycleBehavior : MonoBehaviour {
	public float CycleSeconds = 5.0f;
	public float OffsetSeconds = 0.0f;
	
	private ColorBehavior ColorBehavior;
	private float nextCycleTime;
	
	void Start () {
		ColorBehavior = GetComponent<ColorBehavior>();
		nextCycleTime = OffsetSeconds + Time.time + CycleSeconds;
	}
	
	void Update () {
		if (Time.time >= nextCycleTime) {
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
		ColorBehavior.SetColor (Next (ColorBehavior.Color));
		nextCycleTime = Time.time + CycleSeconds;
	}
}
