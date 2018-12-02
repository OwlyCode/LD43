using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GodBehavior : MonoBehaviour {
    public float anger = 0.8f;
    public float angerShakingRatio = 4f;
    public float patience = 12f;

    bool waiting = false;
    bool leaving = false;
    bool endingTheWorld = false;

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
        UpdateSkyColor();
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
                GameObject.Find("/Global").GetComponent<GameLogic>().TriggerNextWave();
            }
        }

        if (leaving)
        {
            transform.position = Vector2.MoveTowards(transform.position, new Vector2(0, -200), 0.5f);
        }

        if (endingTheWorld)
        {
            leftEyeGlow.transform.localScale = new Vector2(1, 1);
            rightEyeGlow.transform.localScale = new Vector2(1, 1);
        }


        if (!leaving && !endingTheWorld)
        {
            Vector3 pointA = new Vector3(15, 72, 0);
            Vector3 pointB = new Vector3(-15, 72, 1);
            transform.position = Vector3.Lerp(pointA, pointB, Mathf.PingPong(Time.time / 3, 1));
        }
    }

    public void EndTheWorld()
    {
        endingTheWorld = true;
        Shake();
    }

    public void Leave()
    {
        leaving = true;
    }

    public void RequestSacrifice()
    {
        waiting = true;
        timeWaited = 0f;
        bubble.SetActive(true);
    }

    void UpdateSkyColor()
    {
        GameObject camera = GameObject.Find("/Main Camera");

        camera.GetComponent<Camera>().backgroundColor = Color.Lerp(new Color(173f / 255, 95f / 255, 26f / 255), new Color(172f / 255, 50f / 255, 50f / 255), anger - 0.3f);
    }

    void StopWaiting()
    {
        timeWaited = 0f;
        waiting = false;
        bubble.SetActive(false);

        GameObject leftPillar = GameObject.Find("/Temple/LeftPillar");
        GameObject rightPillar = GameObject.Find("/Temple/RightPillar");

        leftPillar.GetComponent<Pillar>().currentSolution = null;
        rightPillar.GetComponent<Pillar>().currentSolution = null;
        leftPillar.GetComponent<Pillar>().StopBlinking();
        rightPillar.GetComponent<Pillar>().StopBlinking();

        foreach (Transform child in leftPillar.transform)
        {
            GameObject.Destroy(child.gameObject);
        }

        foreach (Transform child in rightPillar.transform)
        {
            GameObject.Destroy(child.gameObject);
        }

        UpdateSkyColor();
    }

    void CheckEnd()
    {
        if (anger > 1.0f)
        {
            GameObject.Find("/Global").GetComponent<GameLogic>().Loose();
        }

        if (anger < 0.3f)
        {
            GameObject.Find("/Global").GetComponent<GameLogic>().Win();
        }
    }

    public void Anger()
    {
        GameObject.Find("Sound/Angry").GetComponent<AudioSource>().Play();
        GameObject.Find("Temple").GetComponent<FlashObject>().Flash(Color.red);
        this.anger += 0.1f;
        StopWaiting();
        CheckEnd();
    }

    public void Soften()
    {
        GameObject.Find("Sound/Pleased").GetComponent<AudioSource>().Play();
        GameObject.Find("Temple").GetComponent<FlashObject>().Flash(Color.green);
        this.anger -= 0.1f;
        this.patience = Mathf.Max(this.patience * 0.6f, 3f);
        StopWaiting();
        CheckEnd();
    }

    void StartShaking()
    {
        StartCoroutine(Shake());
    }

    IEnumerator Shake()
    {
        while (true)
        {
            if (waiting || endingTheWorld)
            {
                float amplitude = endingTheWorld ? 4.0f : timeWaited / patience;
                transform.rotation = Quaternion.identity;
                transform.Rotate(0f, 0f, amplitude * angerShakingRatio);
                yield return new WaitForSeconds(0.05f);
                transform.Rotate(0f, 0f, -amplitude * angerShakingRatio * 2);
                yield return new WaitForSeconds(0.05f);
            } else {
                transform.rotation = Quaternion.identity;
                yield return new WaitForSeconds(0.05f);
            }
        }
    }

    void SoftlyNod()
    {

    }

    void ShakeHead()
    {

    }
}
