using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ClickToScene : MonoBehaviour {
    public string scene;

    private void Start()
    {
        Screen.SetResolution(800, 800, FullScreenMode.Windowed, 60);
    }

    void Update () {
		if (Input.GetMouseButtonDown(0))
        {
            SceneManager.LoadScene(scene);
        }
	}
}
