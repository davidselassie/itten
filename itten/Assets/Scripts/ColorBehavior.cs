using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

[RequireComponent(typeof(Rigidbody2D)),
 RequireComponent(typeof(Collider2D))]
public class ColorBehavior : MonoBehaviour {
	// Color and alpha are extracted from the color of the renderers.
	// Set the starting color in the GUI on the renderers.
	public GelColor Color {
		get;
		private set;
	}
	private float Alpha;

    public Collider2D[] Colliders {
        get;
        private set;
    }
	public Collider2D[] Triggers {
		get;
		private set;
	}

	public Rigidbody2D Rigidbody {
		get;
		private set;
	}
	private PlatformerCharacter2D PlatformerCharacter;

	private SpriteRenderer[] SpriteRenderers;
	private Text[] Texts;
	private BlendModes.BlendModeEffect[] BMEs;
	private ParticleSystem[] PSs;

	private Joint2D[] JoinedJoints = new Joint2D[0];

	void Awake () {
		Collider2D[] allColliders = GetComponentsInChildren<Collider2D>();
		Colliders = allColliders.Where(collider => !collider.isTrigger).ToArray();
		if (Colliders.Length < 1) {
			Debug.LogError("ColorBehavior has no physical colliders!", gameObject);
		}
		Triggers = allColliders.Where(collider => collider.isTrigger).ToArray();
		if (Triggers.Length < 1) {
			Debug.LogError("ColorBehavior has no overlap trigger colliders!", gameObject);
		}
		if (Colliders.Length != Triggers.Length) {
			Debug.LogError("ColorBehavior is missing a trigger or physical collider; unequal counts!", gameObject);
		}

		SpriteRenderers = GetComponentsInChildren<SpriteRenderer>();
		Texts = GetComponentsInChildren<Text>();
		BMEs = GetComponentsInChildren<BlendModes.BlendModeEffect>();
		PSs = GetComponentsInChildren<ParticleSystem>();

		Rigidbody = GetComponent<Rigidbody2D>();
        PlatformerCharacter = GetComponent<PlatformerCharacter2D>();

		// Use the first renderer we find to select the color and alpha.
		if (SpriteRenderers.Length > 0) {
			Color = GelColorExtensions.FromRenderColor (SpriteRenderers[0].color);
			Alpha = SpriteRenderers[0].color.a;
		} else if (Texts.Length > 0) {
			Color = GelColorExtensions.FromRenderColor (Texts[0].color);
			Alpha = Texts[0].color.a;
		} else if (BMEs.Length > 0) {
			Color = GelColorExtensions.FromRenderColor (BMEs[0].TintColor);
			Alpha = BMEs[0].TintColor.a;
		}
    }

    void Start () {
		SetColor (Color);
    }

	public void SetColor (GelColor toColor) {
		Color = toColor;

		UpdateRendererColor ();

		ColorBehavior[] CBs = FindObjectsOfType (typeof(ColorBehavior)) as ColorBehavior[];
		ResetCollidability (CBs);
		CreateDestroyJoints (CBs);
		CleanupBackjoints (CBs);
	}

	public bool IsJoined () {
		return JoinedJoints.Length > 0;
	}

	public bool ShouldCollide (ColorBehavior that) {
		return Color.Mix (that.Color) == GelColor.Black;
	}

	private void SetCollidability (ColorBehavior that, bool enabled) {
		foreach (Collider2D a in Colliders) {
			foreach (Collider2D b in that.Colliders) {
				Physics2D.IgnoreCollision(a, b, !enabled);
			}
		}
	}

	private void ResetCollidability (ColorBehavior[] CBs) {
		foreach (ColorBehavior CB in CBs) {
			if (CB != this) {
				SetCollidability(CB, ShouldCollide (CB));
			}
		}
	}

	private bool IsOverlapping (ColorBehavior that) {
		foreach (Collider2D a in Triggers) {
			Collider2D[] allOverlap = Physics2D.OverlapPointAll(a.bounds.center);
			foreach (Collider2D c in allOverlap) {
				if (that.Triggers.Contains(c)) {
					return true;
				}
			}
		}
		return false;
	}

	public void CreateDestroyJoints () {
		ColorBehavior[] CBs = FindObjectsOfType (typeof(ColorBehavior)) as ColorBehavior[];
		CreateDestroyJoints (CBs);
	}

	private void CreateDestroyJoints (ColorBehavior[] CBs) {
		bool wasJoined = IsJoined ();
		foreach (Joint2D joinedJoint in JoinedJoints) {
			Destroy (joinedJoint);
		}

		JoinedJoints = CBs.Where (CB => {
			return CB != this && ShouldCollide (CB) && IsOverlapping (CB);
		}).Select (CB => {
			DistanceJoint2D newJoint = gameObject.AddComponent<DistanceJoint2D> ();
			// Explicitly enable collisions. They're disabled by default and make trigger overlap detection not work.
			newJoint.enableCollision = true;
			newJoint.connectedBody = CB.Rigidbody;
			// Spring zeros to current position in overlapped object's space.
			newJoint.connectedAnchor = CB.gameObject.transform.InverseTransformPoint(
				gameObject.transform.position);
			newJoint.distance = 0.0f;

			// Side effect!
			SetCollidability (CB, false);

			return newJoint;
		}).ToArray ();

		// Allow the player one jump after becoming un-joined.
		if (PlatformerCharacter != null && wasJoined && !IsJoined ()) {
			PlatformerCharacter.ForceAllowJump = true;
		}
	}

	private void CleanupBackjoints (ColorBehavior[] CBs) {
		foreach (ColorBehavior CB in CBs) {
			if (CB != this && IsOverlapping (CB)) {
				CB.CreateDestroyJoints ();
			}
		}
	}

	private void UpdateRendererColor () {
		Color renderColor = Color.GetRenderColor ();
		renderColor.a = Alpha;
		foreach (SpriteRenderer spriteRenderer in SpriteRenderers) {
			spriteRenderer.color = renderColor;
		}
		foreach (Text text in Texts) {
			text.color = renderColor;
		}
		foreach (BlendModes.BlendModeEffect BME in BMEs) {
			BME.TintColor = renderColor;
		}
		foreach (ParticleSystem PS in PSs) {
			PS.startColor = renderColor;
		}
	}
}
