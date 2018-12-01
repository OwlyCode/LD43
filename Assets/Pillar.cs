using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pillar : MonoBehaviour {

    public string currentSolution;

    void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0) && currentSolution != null){
            GameObject.Find("/Global").GetComponent<GameLogic>().Solve(currentSolution);
        }
    }
}
