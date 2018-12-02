using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pillar : MonoBehaviour {

    public string currentSolution;
    bool blinking = false;

    private void Start()
    {
        StartCoroutine(Blink());
    }

    void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0) && currentSolution != null){
            GameObject.Find("/Global").GetComponent<GameLogic>().Solve(currentSolution);
        }
    }

    public void StartBlinking()
    {
        this.blinking = true;
        StartCoroutine(Blink());
    }

    public void StopBlinking()
    {
        this.blinking = false;
    }

    IEnumerator Blink()
    {
        while(blinking)
        {
            GetComponent<SpriteRenderer>().color = new Color(1, 1, 0);
            yield return  new WaitForSeconds(0.1f);
            GetComponent<SpriteRenderer>().color = Color.white;
            yield return new WaitForSeconds(0.1f);
        }
    }
}
