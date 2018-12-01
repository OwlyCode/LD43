﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lightning : MonoBehaviour {

    public GameObject target;

	// Use this for initialization
	void Start () {
        StartCoroutine(Strike());	
	}

    IEnumerator Strike()
    {
        float add = 0.2f;
        Object.Destroy(target);

        for (int i=0; i < 30; i++)
        {
            yield return new WaitForSeconds(0.01f);
            transform.localScale = new Vector2(1 + add, 1);
            add = add * -1;
            
        }

        Object.Destroy(gameObject);
    }
}