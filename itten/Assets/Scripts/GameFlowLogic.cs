using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class GameFlowLogic : MonoBehaviour {
	public AudioSource LoadLevelAudioSource;

	void Start () {
		DontDestroyOnLoad(gameObject);

		// Start off by advancing from the init level to the first one.
		AdvanceLevel ();
	}

	// Levels are advanced in order in the application. Skip level 0 (the Init level).
	public void AdvanceLevel () {
		int nextLevel = (Application.loadedLevel + 1) % Application.levelCount;
		if (nextLevel == 0) {
			nextLevel = 1;
		}
		Application.LoadLevel(nextLevel);
	}

	public void RestartLevel () {
		Application.LoadLevel(Application.loadedLevel);
	}
}
