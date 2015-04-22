using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

[RequireComponent(typeof(Rigidbody2D)),
 RequireComponent(typeof(Collider2D)),
 RequireComponent(typeof(SpriteRenderer))]
public class ColorBehavior : MonoBehaviour {
	// Color is extracted from the color of the renderers. Set the starting color in the GUI on the renderers.
	public GelColor Color {
		get;
		private set;
	}

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
	private SpriteRenderer[] Renderers;

	private Joint2D EmbedJoint = null;

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
		Renderers = GetComponentsInChildren<SpriteRenderer>();
		Rigidbody = GetComponent<Rigidbody2D>();
    }

    void Start () {
		// Use the first renderer we find to pick the color.
		Color = GelColorExtensions.FromRenderColor (Renderers[0].color);
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

	private bool IsOverlapping (ColorBehavior that) {
		foreach (Collider2D a in Triggers) {
			foreach (Collider2D b in that.Triggers) {
				if (b.bounds.Contains(a.bounds.center)) {
					return true;
				}
			}
		}
		return false;
	}

	private void ReckonOverlapping (ColorBehavior[] CBs) {
		foreach (ColorBehavior CB in CBs) {
			if (CB != this) {
				if (ShouldCollide (CB) && IsOverlapping (CB)) {
					// If we've already got a constraint and we're still stuck after a color change, keep using that constraint.
					// You just cycled through to another color that also gets stuck.
					// TODO: Figure out what to do if you should be embedded in multiple other blocks.
					if (EmbedJoint == null) {
						DistanceJoint2D newJoint = gameObject.AddComponent<DistanceJoint2D>();
						// Explicitly enable collisions. They're disabled by default and make trigger overlap detection not work.
						newJoint.enableCollision = true;
						newJoint.connectedBody = CB.Rigidbody;
						// Spring zeros to current position in overlapped object's space.
						newJoint.connectedAnchor = CB.gameObject.transform.InverseTransformPoint(
							gameObject.transform.position);
						newJoint.distance = 0.0f;

						EmbedJoint = newJoint;
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
		foreach (SpriteRenderer renderer in Renderers) {
			renderer.color = Color.GetRenderColor ();
		}
	}
}
