using UnityEngine;
using System.Collections;

// These components are required on self or some child of this object.
// I don't think there's a RequireComponentOnChildren.
//[RequireComponent(typeof(Collider2D)), RequireComponent(typeof(SpriteRenderer))]
public class ColorBehavior : MonoBehaviour {
	public GelColor Color = GelColor.Blue;

    public Collider2D[] Colliders {
        get;
        private set;
    }
	private SpriteRenderer[] Renderers;

	void Awake () {
		Colliders = GetComponentsInChildren<Collider2D>();
		Renderers = GetComponentsInChildren<SpriteRenderer>();
    }

    void Start () {
		ReckonColorChange ();
    }

	// Must call after setting Color to make sure all physical interactions are correct. Pitty we can't use C# properties.
	public void ReckonColorChange () {
		AllowCollisions ();
		EmbedOverlapping ();
		UpdateRendererColors ();
	}
	
	private static bool ShouldCollide (GelColor one, GelColor two) {
		return one.Mix (two) == GelColor.Black;
	}

    private void AllowCollisions () {
        ColorBehavior[] CBs = FindObjectsOfType(typeof(ColorBehavior)) as ColorBehavior[];
		foreach (Collider2D collider in Colliders) {
			foreach (ColorBehavior CB in CBs) {
				foreach (Collider2D colliderCB in CB.Colliders) {
					Physics2D.IgnoreCollision (collider,
					                           colliderCB,
					                           !ShouldCollide (Color, CB.Color));
				}
			}
        }
    }

	private void EmbedOverlapping () {

	}

	private void UpdateRendererColors () {
		foreach (SpriteRenderer renderer in Renderers) {
			renderer.color = Color.RenderColor ();
		}
	}
}
