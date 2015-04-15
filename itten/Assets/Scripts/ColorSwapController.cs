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

	void Swap () {
		if (colorBehavior.Color == ColorBehavior.ColorEnum.Blue) {
			colorBehavior.Color = ColorBehavior.ColorEnum.Red;
		} else {
			colorBehavior.Color = ColorBehavior.ColorEnum.Blue;
		}
		Debug.Log ("Color swapped", colorBehavior);
		colorBehavior.ReckonColorChange ();
	}
}
