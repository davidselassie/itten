using UnityEngine;
using System.Collections;

[RequireComponent(typeof(ColorBehavior))]
public class ColorSwapController : MonoBehaviour {
	public GameObject BurstPrefab;
	public Transform BurstTarget;

	private ColorBehavior ColorBehavior;

	void Start () {
		ColorBehavior = GetComponent<ColorBehavior>();
		if (BurstPrefab == null) {
			Debug.LogWarning ("ColorSwapController doesn't have a burst prefab.", gameObject);
		}
		if (BurstTarget == null) {
			BurstTarget = gameObject.transform;
		}
	}

	void Update () {
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
		ColorBehavior.SetColor (Next (ColorBehavior.Color));
		Burst ();
	}

	void Burst () {
		if (BurstPrefab != null) {
			GameObject burst = Instantiate(BurstPrefab, BurstTarget.position, BurstTarget.rotation) as GameObject;
			burst.transform.parent = gameObject.transform;
		}
	}
}
