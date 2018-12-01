using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GodBehavior : MonoBehaviour {
    public float anger = 0.8f;
    public float angerShakingRatio = 4f;
    public float patience = 5f;

    bool waiting = false;
    float timeWaited = 0f;

    GameObject leftEyeGlow;
    GameObject rightEyeGlow;

    void Start () {
        StartShaking();

        leftEyeGlow = GameObject.Find("God/LeftEye/Glow");
        rightEyeGlow = GameObject.Find("God/RightEye/Glow");
 
        leftEyeGlow.transform.localScale = Vector2.zero;
        rightEyeGlow.transform.localScale = Vector2.zero;

        RequestSacrifice();
    }

    void Update()
    {
        float eyeGlowSize = timeWaited / patience;

        leftEyeGlow.transform.localScale = new Vector2(eyeGlowSize, eyeGlowSize);
        rightEyeGlow.transform.localScale = new Vector2(eyeGlowSize, eyeGlowSize);

        if (waiting)
        {
            timeWaited += Time.deltaTime;

            if (timeWaited > patience)
            {
                timeWaited = 0f;
                waiting = false;
                anger += 0.1f;
            }
        }
    }

    void RequestSacrifice()
    {
        waiting = true;
        timeWaited = 0f;
    }

    void StartShaking()
    {
        StartCoroutine(Shake());
    }

    IEnumerator Shake()
    {
        while (true)
        {
            if (!waiting)
            {
                transform.rotation = Quaternion.identity;
                transform.Rotate(0f, 0f, anger * angerShakingRatio);
                yield return new WaitForSeconds(0.05f);
                transform.Rotate(0f, 0f, -anger * angerShakingRatio * 2);
                yield return new WaitForSeconds(0.05f);
            } else {
                transform.rotation = Quaternion.identity;
                yield return new WaitForSeconds(0.05f);
            }
        }
    }
}
