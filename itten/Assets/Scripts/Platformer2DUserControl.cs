using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

[RequireComponent(typeof(PlatformerCharacter2D))]
public class Platformer2DUserControl : MonoBehaviour
{
	private PlatformerCharacter2D PlatformerCharacter;
	private bool DoJump;

	private void Awake () {
		PlatformerCharacter = GetComponent<PlatformerCharacter2D>();
	}
	
	private void Update () {
		// Read the jump input in Update so button presses aren't missed.
		if (!DoJump) {
			DoJump = Input.GetButtonDown ("Jump");
		}
	}
	
	private void FixedUpdate () {
		float h = Input.GetAxis ("Run");
		PlatformerCharacter.Move (h);
		if (DoJump) {
			PlatformerCharacter.Jump ();
			DoJump = false;
		}
	}
}
