using UnityEngine;
using System.Collections;

public class GameFlowLogic : MonoBehaviour {
	void Start () {
		DontDestroyOnLoad(gameObject);
	}

	// Levels are advanced in order in the application.
	public void AdvanceLevel () {
		Application.LoadLevel((Application.loadedLevel + 1) % Application.levelCount);
	}

	public void RestartLevel () {
		Application.LoadLevel(Application.loadedLevel);
	}
}
