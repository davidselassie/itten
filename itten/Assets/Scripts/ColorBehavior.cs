using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Collider2D))]
public class ColorBehavior : MonoBehaviour {
    public enum ColorEnum {
        Blue,
        Red
    };

	public ColorEnum Color = ColorEnum.Blue;
    public Collider2D Collider2D {
        get;
        private set;
    }
	private Renderer Renderer;

    void Start () {
        Collider2D = GetComponent<Collider2D>();
		Renderer = GetComponent<Renderer>();
		ReckonColorChange ();
    }

	public void ReckonColorChange () {
		AllowCollisions ();
		if (Color == ColorEnum.Blue) {
			Renderer.material.color = UnityEngine.Color.blue;
		} else {
			Renderer.material.color = UnityEngine.Color.red;
		}
	}

    private void AllowCollisions () {
        ColorBehavior[] CBs = FindObjectsOfType(typeof(ColorBehavior)) as ColorBehavior[];
        foreach (ColorBehavior CB in CBs) {
            Physics2D.IgnoreCollision (Collider2D,
                                       CB.Collider2D,
                                       !ShouldCollide (Color, CB.Color));
        }
    }

    private static bool ShouldCollide (ColorEnum one, ColorEnum two) {
        return one != two;
    }
}
