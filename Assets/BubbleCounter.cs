using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class BubbleCounter : MonoBehaviour {

	// Use this for initialization
	void Start ()
    {
        GetComponent<MeshRenderer>().sortingLayerName = "Front";
        GetComponent<MeshRenderer>().sortingOrder = 50;
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
