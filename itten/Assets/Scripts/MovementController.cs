using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
public class MovementController : MonoBehaviour {
	new private Rigidbody2D rigidbody2D;
	
	void Start () {
		rigidbody2D = GetComponent<Rigidbody2D>();
	}

	void FixedUpdate () {
		if (Input.GetKey ("right")) {
			rigidbody2D.AddForce(new Vector2(1.0f, 0.0f));
		}
		if (Input.GetKey ("left")) {
			rigidbody2D.AddForce(new Vector2(-1.0f, 0.0f));
		}
		if (Input.GetKey ("up")) {
			rigidbody2D.AddForce(new Vector2(0.0f, 1.0f));
		}
		if (Input.GetKey ("down")) {
			rigidbody2D.AddForce(new Vector2(0.0f, -1.0f));
		}
	}
}
