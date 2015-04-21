using UnityEngine;
using System.Collections;
using System;

public class ElevatorComponent : MonoBehaviour {
	void FixedUpdate () {
		gameObject.transform.Translate(new Vector2(0.0f, (float)Math.Sin(Time.fixedTime) / 50.0f));
	}
}
