using UnityEngine;
using System.Collections;

public class LevelBoundsBehavior : MonoBehaviour {
	private GameFlowLogic FlowLogic;
	
	void Start () {
		Collider2D collider = GetComponent<Collider2D>();
		if (!collider.isTrigger) {
			Debug.LogError ("LevelBoundsBehavior not attached to object with trigger collider!", gameObject);
		}
		FlowLogic = FindObjectOfType(typeof(GameFlowLogic)) as GameFlowLogic;
		if (FlowLogic == null) {
			Debug.LogWarning ("No GameFlowLogic in scene. Out-of-bounds will restart level.");
		}
	}

	void OnTriggerEnter2D (Collider2D other) {
		if (other.gameObject.tag == "Player") {
			if (FlowLogic != null) {
				FlowLogic.RestartLevel ();
			} else {
				Application.LoadLevel(Application.loadedLevel);
			}
		}
	}
}
