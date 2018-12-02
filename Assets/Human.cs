using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Human : MonoBehaviour {
    public GameObject rightHand;
    public GameObject leftHand;
    public GameObject head;

    public string rightHandId;
    public string leftHandId;
    public string headId;

    public bool HasAccessory(string id)
    {
        return this.rightHandId == id || this.leftHandId == id || this.headId == id;
    }

    // Use this for initialization
    void Start ()
    {
        if (rightHand)
        {
            Vector3 pos = Vector3.zero;
            GameObject rightHandInstance = Instantiate(rightHand, pos, Quaternion.identity);
            rightHandInstance.transform.parent = gameObject.transform;
            rightHandInstance.transform.localPosition = new Vector3(7.4f, 18.4f, 0);
        }
        if (leftHand)
        {
            Vector3 pos = Vector3.zero;
            GameObject leftHandInstance = Instantiate(leftHand, pos, Quaternion.identity);
            leftHandInstance.transform.parent = gameObject.transform;
            leftHandInstance.transform.localPosition = new Vector3(-8, 18.7f, 0);
        }
        if (head)
        {
            Vector3 pos = Vector3.zero;
            GameObject headInstance = Instantiate(head, pos, Quaternion.identity);
            headInstance.transform.parent = gameObject.transform;
            headInstance.transform.localPosition = new Vector3(0, 22.32f, 0);
        }
    }
	
	// Update is called once per frame
	void Update () {
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.y/100f);
	}
}
