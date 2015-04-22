using UnityEngine;
using System.Collections;

public class ColorShiftBurst : MonoBehaviour {

	public GameObject burstPrefab;
	public Transform burstTarget;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown ("m")) 
		{
			GameObject burstInstance;
			burstInstance = Instantiate(burstPrefab, burstTarget.position, burstTarget.rotation) as GameObject;
			burstInstance.transform.parent = transform;
		}
	}
}
