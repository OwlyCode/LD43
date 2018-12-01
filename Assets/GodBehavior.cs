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
    GameObject bubble;

    void Start () {
        StartShaking();

        leftEyeGlow = GameObject.Find("God/LeftEye/Glow");
        rightEyeGlow = GameObject.Find("God/RightEye/Glow");
        bubble = GameObject.Find("/Bubble");
        bubble.SetActive(false);

        leftEyeGlow.transform.localScale = Vector2.zero;
        rightEyeGlow.transform.localScale = Vector2.zero;
    }

    void Update()
    {
        float eyeGlowSize = timeWaited / patience;

        leftEyeGlow.transform.localScale = new Vector2(eyeGlowSize, eyeGlowSize);
        rightEyeGlow.transform.localScale = new Vector2(eyeGlowSize, eyeGlowSize);

        transform.localScale = new Vector2(.5f + anger, .5f + anger);

        if (waiting)
        {
            timeWaited += Time.deltaTime;

            if (timeWaited > patience)
            {
                Anger();
            }
        }
    }

    public void RequestSacrifice()
    {
        waiting = true;
        timeWaited = 0f;
        bubble.SetActive(true);
    }

    void stopWaiting()
    {
        timeWaited = 0f;
        waiting = false;
        bubble.SetActive(false);

        GameObject leftPillar = GameObject.Find("/Temple/LeftPillar");
        GameObject rightPillar = GameObject.Find("/Temple/RightPillar");

        foreach (Transform child in leftPillar.transform)
        {
            GameObject.Destroy(child.gameObject);
        }

        foreach (Transform child in rightPillar.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
    }

    public void Anger()
    {
        this.anger += 0.1f;
        stopWaiting();
    }

    public void Soften()
    {
        this.anger -= 0.1f;
        stopWaiting();
    }

    void StartShaking()
    {
        StartCoroutine(Shake());
    }

    IEnumerator Shake()
    {
        while (true)
        {
            if (waiting)
            {
                transform.rotation = Quaternion.identity;
                transform.Rotate(0f, 0f, (timeWaited/patience) * angerShakingRatio);
                yield return new WaitForSeconds(0.05f);
                transform.Rotate(0f, 0f, -(timeWaited / patience) * angerShakingRatio * 2);
                yield return new WaitForSeconds(0.05f);
            } else {
                transform.rotation = Quaternion.identity;
                yield return new WaitForSeconds(0.05f);
            }
        }
    }
}
