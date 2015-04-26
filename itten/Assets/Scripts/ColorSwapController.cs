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
		if (Input.GetKeyDown ("r")) {
			SwapTo (GelColor.Red);
		} else if (Input.GetKeyDown ("g")) {
			SwapTo (GelColor.Green);
		} else if (Input.GetKeyDown ("b")) {
			SwapTo (GelColor.Blue);
		}
	}

	private void SwapTo (GelColor next) {
		ColorBehavior.SetColor (next);
		Burst ();
	}

	private void Burst () {
		if (BurstPrefab != null) {
			GameObject burst = Instantiate(BurstPrefab, BurstTarget.position, BurstTarget.rotation) as GameObject;
			burst.transform.parent = gameObject.transform;
		}
	}
}
