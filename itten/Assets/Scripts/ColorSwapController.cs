using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

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
		if (Input.GetButtonDown ("Red") && ColorBehavior.Color != GelColor.Red) {
			SwapTo (GelColor.Red);
		} else if (Input.GetButtonDown ("Green") && ColorBehavior.Color != GelColor.Green) {
			SwapTo (GelColor.Green);
		} else if (Input.GetButtonDown ("Blue") && ColorBehavior.Color != GelColor.Blue) {
			SwapTo (GelColor.Blue);
		}
	}

	private void SwapTo (GelColor next) {
		ColorBehavior.SetColor (next);
		Burst (next);
	}

	private void Burst (GelColor color) {
		if (BurstPrefab != null) {
			GameObject burst = Instantiate(BurstPrefab, BurstTarget.position, BurstTarget.rotation) as GameObject;
			burst.transform.parent = gameObject.transform;
			foreach (SpriteRenderer renderer in burst.GetComponentsInChildren<SpriteRenderer>()) {
				renderer.color = color.GetRenderColor ();
			}
		}
	}
}
