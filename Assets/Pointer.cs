using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pointer : MonoBehaviour {

    // Use this for initialization
    void Start()
    {
        StartCoroutine(Click());
    }

    IEnumerator Click()
    {
        while(true)
        {
            yield return new WaitForSeconds(1.0f);
            transform.position = transform.position + new Vector3(-2, 0, 0);
            transform.Rotate(0, 0, 2);
            yield return new WaitForSeconds(0.1f);
            transform.position = transform.position - new Vector3(-2, 0, 0);
            transform.rotation = Quaternion.identity;
        }
    }
}
