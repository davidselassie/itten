﻿using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Collider2D))]
public abstract class AbstractLevelBehavior : MonoBehaviour {
	protected GameFlowLogic FlowLogic;
	
	void Start () {
		Collider2D collider = GetComponent<Collider2D>();
		if (!collider.isTrigger) {
			Debug.LogError ("LevelBehavior not attached to object with trigger collider!", gameObject);
		}
		FlowLogic = FindObjectOfType(typeof(GameFlowLogic)) as GameFlowLogic;
		if (FlowLogic == null) {
			Debug.LogWarning ("No GameFlowLogic in scene. Level behavior collision will restart level.");
		}
	}
	
	void OnTriggerEnter2D (Collider2D other) {
		if (other.gameObject.tag == "Player") {
			if (FlowLogic != null) {
				Action ();
			} else {
				Application.LoadLevel(Application.loadedLevel);
			}
		}
	}
	
	abstract public void Action ();
}