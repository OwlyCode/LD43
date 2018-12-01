using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spot : MonoBehaviour {
    public GameObject assignedHuman;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (assignedHuman && assignedHuman.transform.position != transform.position)
        {
            assignedHuman.transform.position = Vector2.MoveTowards(assignedHuman.transform.position, transform.position, 2f);
        }
	}
}
