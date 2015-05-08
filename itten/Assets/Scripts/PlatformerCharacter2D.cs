using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlatformerCharacter2D : MonoBehaviour
{
	[SerializeField] private float MaxXSpeed = 10.0f;
	[SerializeField] private float JumpForce = 1200.0f;

	private bool OnGround = false;
	public bool ForceAllowJump = false;

	public Transform GroundCheck;
	private Animator Animator;
	private Rigidbody2D Rigidbody;
	private ColorBehavior ColorBehavior;

	public AudioSource RunAudioPlayer;
	public AudioSource JumpAudioPlayer;

	private void Awake () {
		Animator = GetComponent<Animator>();
		Rigidbody = GetComponent<Rigidbody2D>();
		ColorBehavior = GetComponent<ColorBehavior>();
	}

	private void FixedUpdate () {
		OnGround = false;
		Collider2D[] overlapping = Physics2D.OverlapCircleAll(GroundCheck.position,
		                                                          0.2f,
		                                                          -1); // All layers.
		foreach (Collider2D overlap in overlapping) {
			if (overlap.gameObject != gameObject) {
				ColorBehavior otherCB = overlap.GetComponent<ColorBehavior>();
				if (ColorBehavior == null ||
				    otherCB == null ||
				    ColorBehavior.ShouldCollide(otherCB)) {
					OnGround = true;
					break;
				}
			}
		}

		if (Animator != null) {
			Animator.SetFloat("vSpeed", Rigidbody.velocity.y);
			Animator.SetBool("Ground", OnGround);
		}
	}

	public void Move (float xMagnitude) {
		if (Animator != null) {
			Animator.SetFloat("Speed", Mathf.Abs(xMagnitude));
		}

		Rigidbody.velocity = new Vector2(xMagnitude * MaxXSpeed, Rigidbody.velocity.y);

		if (RunAudioPlayer != null) {
			bool shouldPlay = (xMagnitude != 0.0f && OnGround);
			if (shouldPlay && !RunAudioPlayer.isPlaying) {
				RunAudioPlayer.Play ();
			} else if (!shouldPlay && RunAudioPlayer.isPlaying) {
				RunAudioPlayer.Stop ();
			}
		}

		Flip (xMagnitude);
	}

	public void Jump () {
		if ((OnGround || ForceAllowJump) && !ColorBehavior.IsJoined ()) {
			OnGround = ForceAllowJump = false;

			Rigidbody.velocity = new Vector2(Rigidbody.velocity.x, 0.0f);
			Rigidbody.AddForce(new Vector2(0.0f, JumpForce));

			if (JumpAudioPlayer != null) {
				JumpAudioPlayer.Play ();
			}
		}
	}

	private void Flip (float xMagnitude) {
		Vector3 localScale = gameObject.transform.localScale;
		if (xMagnitude > 0.0f && localScale.x < 0.0f ||
		    xMagnitude < 0.0f && localScale.x > 0.0f) {
			localScale.x *= -1;
			gameObject.transform.localScale = localScale;
		}
	}
}
