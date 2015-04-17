using UnityEngine;
using System.Collections;

public class LevelExitBehavior : MonoBehaviour {
	private GameFlowLogic FlowLogic;

	void Start () {
		FlowLogic = FindObjectOfType(typeof(GameFlowLogic)) as GameFlowLogic;
		if (FlowLogic == null) {
			Debug.LogWarning("No GameFlowLogic in scene! Level exit will restart level.");
		}
	}

	void OnCollisionEnter2D (Collision2D collision) {
		if (collision.gameObject.tag == "Player") {
			if (FlowLogic != null) {
				FlowLogic.AdvanceLevel ();
			} else {
				Application.LoadLevel(Application.loadedLevel);
			}
		}
	}
}
