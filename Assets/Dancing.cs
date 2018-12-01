using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dancing : MonoBehaviour {

    public GameObject beatSource;
    public float bpm = 120f;

    private float nextBeat = 0;
    private float lastTime = 0;
    private float inclination = 2f;

    // Use this for initialization
    void Start () {
        transform.Rotate(Vector3.forward * inclination);
    }

    // Update is called once per frame
    void Update () {
        if(!beatSource) {
            return;
        }

        AudioSource audio = beatSource.GetComponent<AudioSource>();

        if (audio.time < lastTime)
        {
            nextBeat = 0;
        }

        if(audio.time > nextBeat)
        {
            inclination = -inclination;
            transform.Rotate(Vector3.forward * inclination * 2f);
            nextBeat += 60f / bpm;
        }

        lastTime = audio.time;
	}
}
