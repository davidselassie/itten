using UnityEngine;
using System.Collections;

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

	public void ReckonColorChange () {
		AllowCollisions ();
		UpdateRendererColors ();
	}

    private void AllowCollisions () {
        ColorBehavior[] CBs = FindObjectsOfType(typeof(ColorBehavior)) as ColorBehavior[];
		foreach (Collider2D collider in Colliders) {
			foreach (ColorBehavior CB in CBs) {
				foreach (Collider2D colliderCB in CB.Colliders) {
					Physics2D.IgnoreCollision (collider,
					                           colliderCB,
					                           !Color.ShouldCollide (CB.Color));
				}
			}
        }
    }

	private void UpdateRendererColors () {
		foreach (SpriteRenderer renderer in Renderers) {
			renderer.color = Color.RenderColor ();
		}
	}
}
