using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// These components are required on self or some child of this object.
// I don't think there's a RequireComponentOnChildren.
//[RequireComponent(typeof(Collider2D)), RequireComponent(typeof(SpriteRenderer))]
public class ColorBehavior : MonoBehaviour {
	// Please call SetColor to modify. C# properties don't appear in Unity GUI.
	public GelColor Color = GelColor.Blue;
	private GelColor ColorBuffer;

    public Collider2D[] Colliders {
        get;
        private set;
    }
	private SpriteRenderer[] Renderers;
	private SpringJoint2D EmbedJoint;

	void Awake () {
		Colliders = GetComponentsInChildren<Collider2D>();
		Renderers = GetComponentsInChildren<SpriteRenderer>();
		// Start with no color change.
		ColorBuffer = Color;
    }

    void Start () {
		SwapColorBuffer ();
    }

	private void SetIgnoredCollisions (IEnumerable<Collider2D> ignoreWith) {
		ColorBehavior[] CBs = FindObjectsOfType (typeof(ColorBehavior)) as ColorBehavior[];
		// First, reset all collisions to allowed.
		foreach (ColorBehavior CB in CBs) {
			foreach (Collider2D a in Colliders) {
				foreach (Collider2D b in CB.Colliders) {
					Physics2D.IgnoreCollision (a, b, false);
				}
			}
		}
		// Then disallow specificially ignored colliders.
		// Careful that you don't ignore non-ColorBehavior containing colliders, as they won't be reset above.
		foreach (Collider2D b in ignoreWith) {
			foreach (Collider2D a in Colliders) {
				Physics2D.IgnoreCollision(a, b, true);
			}
		}
	}

	public void SetColor (GelColor toColor) {
		if (toColor != Color) {
			// Enable all collisions. The *next* physics frame will have access to all collisions.
			SetIgnoredCollisions (new List<Collider2D>());
			// Now mark that we need to change the color in the *next* physics frame. FixedUpdate does this.
			ColorBuffer = toColor;
		}
	}

	void FixedUpdate () {
		if (ColorBuffer != Color) {
			// Now that we've reset all collisions in the previous physics frame in SetColor, actually apply the correct physics.
			SwapColorBuffer ();
		}
	}

	private void SwapColorBuffer () {
		Color = ColorBuffer;
		UpdateRendererColors ();

		ColorBehavior[] CBs = FindObjectsOfType (typeof(ColorBehavior)) as ColorBehavior[];
		List<Collider2D> ignoreCollisionsWith = new List<Collider2D>();
		ignoreCollisionsWith.AddRange(TransparentColliders (CBs));
		ignoreCollisionsWith.AddRange(OverlappingColliders (CBs));
		// Note that because of list appending, the ignore list is ||-ed effectively.
		SetIgnoredCollisions (ignoreCollisionsWith);
	}
	
	private bool ShouldCollide (ColorBehavior that) {
		return Color.Mix (that.Color) == GelColor.Black;
	}

	private List<Collider2D> TransparentColliders (ColorBehavior[] CBs) {
		List<Collider2D> newIgnores = new List<Collider2D>();
		foreach (ColorBehavior CB in CBs) {
			if (CB != this && !ShouldCollide (CB)) {
				newIgnores.AddRange (CB.Colliders);
			}
		}
		return newIgnores;
	}

	private bool IsTouching (ColorBehavior that) {
		foreach (Collider2D a in Colliders) {
			foreach (Collider2D b in that.Colliders) {
				if (Physics2D.IsTouching(a, b)) {
					return true;
				}
			}
		}
		return false;
	}

	private List<Collider2D> OverlappingColliders (ColorBehavior[] CBs) {
		List<Collider2D> newIgnores = new List<Collider2D>();
		if (EmbedJoint != null) {
			Destroy(EmbedJoint);
			EmbedJoint = null;
		}
		foreach (ColorBehavior CB in CBs) {
			if (CB != this) {
				if (ShouldCollide (CB) && IsTouching (CB)) {
					newIgnores.AddRange (CB.Colliders);
					EmbedJoint = gameObject.AddComponent<SpringJoint2D>();
					EmbedJoint.connectedAnchor = gameObject.transform.position;
					EmbedJoint.enableCollision = false;
					EmbedJoint.distance = 0.0f;
					EmbedJoint.frequency = 20.0f;
					break;
				}
			}
		}
		return newIgnores;
	}

	private void UpdateRendererColors () {
		foreach (SpriteRenderer renderer in Renderers) {
			renderer.color = Color.RenderColor ();
		}
	}
}
