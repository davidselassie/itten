using UnityEngine;
using System.Collections;

public class GameFlowLogic : MonoBehaviour {
	public AudioSource AdvanceAudio;
	public AudioSource RestartAudio;

	void Start () {
		DontDestroyOnLoad(gameObject);

		// Start off by advancing from the init level to the first one.
		AdvanceLevel ();
	}

	// Levels are advanced in order in the application.
	public void AdvanceLevel () {
		Application.LoadLevel((Application.loadedLevel + 1) % Application.levelCount);
		if (AdvanceAudio != null) {
			AdvanceAudio.Play ();
		}
	}

	public void RestartLevel () {
		Application.LoadLevel(Application.loadedLevel);
		if (RestartAudio != null) {
			RestartAudio.Play ();
		}
	}
}
