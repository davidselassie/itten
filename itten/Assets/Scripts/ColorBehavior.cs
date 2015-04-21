using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

[RequireComponent(typeof(Rigidbody2D)),
 RequireComponent(typeof(Collider2D)),
 RequireComponent(typeof(SpriteRenderer))]
public class ColorBehavior : MonoBehaviour {
	// Please call SetColor to modify. C# properties don't appear in Unity GUI.
	public GelColor Color = GelColor.Blue;

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
	private SpriteRenderer Renderer;
	private SpringJoint2D EmbedJoint;

	void Awake () {
		Collider2D[] allColliders = GetComponents<Collider2D>();
		Colliders = allColliders.Where(collider => !collider.isTrigger).ToArray();
		if (Colliders.Length < 1) {
			Debug.LogError("ColorBehavior has no physical colliders!", gameObject);
		}
		Triggers = allColliders.Where(collider => collider.isTrigger).ToArray();
		if (Triggers.Length < 1) {
			Debug.LogError("ColorBehavior has no overlap trigger colliders!", gameObject);
		}
		Renderer = GetComponent<SpriteRenderer>();
		Rigidbody = GetComponent<Rigidbody2D>();
    }

    void Start () {
		SetColor (Color);
    }

	public void SetColor (GelColor toColor) {
		Color = toColor;

		UpdateRendererColor ();

		ColorBehavior[] CBs = FindObjectsOfType (typeof(ColorBehavior)) as ColorBehavior[];
		ResetCollidability (CBs);
		ReckonOverlapping (CBs);
	}
	
	private bool ShouldCollide (ColorBehavior that) {
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

	private bool IsTouching (ColorBehavior that) {
		foreach (Collider2D a in Triggers) {
			foreach (Collider2D b in that.Triggers) {
				if (Physics2D.IsTouching(a, b)) {
					return true;
				}
			}
		}
		return false;
	}

	private void ReckonOverlapping (ColorBehavior[] CBs) {
		foreach (ColorBehavior CB in CBs) {
			if (CB != this) {
				if (ShouldCollide (CB) && IsTouching (CB)) {
					// If we've already got a constraint and we're still stuck after a color change, keep using that constraint.
					// You just cycled through to another color that also gets stuck.
					// TODO: Figure out what to do if you should be embedded in multiple other blocks.
					if (EmbedJoint == null) {
						EmbedJoint = gameObject.AddComponent<SpringJoint2D>();
						// Explicitly enable collisions. They're disabled by default and make trigger overlap detection not work.
						EmbedJoint.enableCollision = true;
						EmbedJoint.connectedBody = CB.Rigidbody;
						// Spring zeros to current position in overlapped object's space.
						EmbedJoint.connectedAnchor = CB.gameObject.transform.InverseTransformPoint(
							gameObject.transform.position);
						EmbedJoint.distance = 0.0f;
						EmbedJoint.frequency = 7.0f;
					}
					// Manually disable collisions on only the physical colliders, not the triggers.
					SetCollidability (CB, false);
                    return;
				}
			}
		}
		// If there were no colors we were embedded in, remove existing constraints.
		if (EmbedJoint != null) {
			Destroy(EmbedJoint);
			EmbedJoint = null;
		}
	}

	private void UpdateRendererColor () {
		Renderer.color = Color.RenderColor ();
	}
}
