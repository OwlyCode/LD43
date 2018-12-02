using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashObject : MonoBehaviour {

    Color color;
    float maxTime;
    float timeLeft;
	
    public void Flash(Color color, float maxTime = 2f)
    {
        this.color = color;
        this.maxTime = maxTime;
        timeLeft = maxTime;
    }

	void Update () {
        if (timeLeft == 0)
        {
            return;
        }

        gameObject.GetComponent<SpriteRenderer>().color = Color.Lerp(Color.white, color, timeLeft / maxTime);

        timeLeft -= Time.deltaTime;

        timeLeft = Mathf.Max(0, timeLeft);
	}
}
