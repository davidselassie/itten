using UnityEngine;
using UnityEditor;
using System.Collections;

public class AutoTileTexture : MonoBehaviour {

	public float tileScale = 2.0f;


	void OnDrawGizmos()
	{
		
		this.gameObject.GetComponent<Renderer>().material.SetTextureScale("_MainTex",new Vector2(this.gameObject.transform.lossyScale.x * tileScale,this.gameObject.transform.lossyScale.y * tileScale))  ;
	}
}
