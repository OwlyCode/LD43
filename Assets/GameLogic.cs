using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLogic : MonoBehaviour {
    public GameObject ironSpear;
    public GameObject goldenSpear;
    public GameObject redHelmet;
    public GameObject blueHelmet;
    public GameObject ironShield;
    public GameObject goldenShield;
    public GameObject human;

    List<GameObject> humans;

    private void Start()
    {
        humans = new List<GameObject>();

        for (int i = 0; i <= 8; i++)
        {
            for (int j=0; j < 2; j++)
            {
                GameObject newHuman = SpawnRandomHuman(i, j);
                humans.Add(newHuman);
                if (!isSolvable(humans, "R1", "H1"))
                {
                    humans.Remove(newHuman);
                    Object.Destroy(newHuman);
                    j--;
                }
            }
        }
    }

    bool isSolvable(List<GameObject> humans, string required, string forbidden)
    {
        List<string> solutions = new List<string>(new[] { "L1", "L2", "R1", "R2", "H1", "H2"});
        List<GameObject> possibleHumans = new List<GameObject>();

        foreach (GameObject h in humans)
        {
            Human human = h.GetComponent<Human>();

            if (human.rightHandId == forbidden || human.leftHandId == forbidden || human.headId == forbidden)
            {
                int rightPos = solutions.IndexOf(human.rightHandId);
                if (rightPos != -1)
                    solutions.RemoveAt(rightPos);

                int leftPos = solutions.IndexOf(human.leftHandId);
                if (leftPos != -1)
                    solutions.RemoveAt(leftPos);

                int headPos = solutions.IndexOf(human.headId);
                if (headPos != -1)
                    solutions.RemoveAt(headPos);
            } else {
                possibleHumans.Add(h);
            }
        }

        foreach (GameObject h in possibleHumans)
        {
            Human human = h.GetComponent<Human>();

            if (human.rightHandId == required || human.leftHandId == required || human.headId == required)
            {
                foreach(string s in solutions)
                {
                    Debug.Log(s);
                }
                return true;
            }
        }

        return false;
    }

    GameObject SpawnRandomHuman(int x, int y)
    {
        return SpawnHuman(new Vector2(-100 + x * 25, -50 + y * 43), Random.Range(0, 3), Random.Range(0, 3), Random.Range(0, 3));
    }

    GameObject SpawnHuman(Vector2 position, int leftHand, int rightHand, int head) {
        GameObject crowd = GameObject.Find("/Crowd");
        GameObject music = GameObject.Find("/loop");

        GameObject humanInstance = GameObject.Instantiate(human);
        Human humanComponent = humanInstance.GetComponent<Human>();

        if (rightHand == 1)
        {
            humanComponent.rightHand = goldenSpear;
        }
        if (rightHand == 2)
        {
            humanComponent.rightHand = ironSpear;
        }

        if (leftHand == 1)
        {
            humanComponent.leftHand = goldenShield;
        }
        if (leftHand == 2)
        {
            humanComponent.leftHand = ironShield;
        }

        if (head == 1)
        {
            humanComponent.head = blueHelmet;
        }
        if (head == 2)
        {
            humanComponent.head = redHelmet;
        }

        humanComponent.leftHandId = "L" + leftHand;
        humanComponent.rightHandId = "R" + rightHand;
        humanComponent.headId = "H" + head;

        humanInstance.GetComponent<Dancing>().beatSource = music;
        humanInstance.transform.parent = crowd.transform;
        humanInstance.transform.localPosition = position;

        return humanInstance;
	}
}
