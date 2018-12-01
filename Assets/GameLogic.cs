using System;
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
    public GameObject spot;

    List<GameObject> humans;

    private void Start()
    {
        humans = new List<GameObject>();
    }

    private void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            Generate();
        }
    }

    void Generate()
    {
        foreach(GameObject h in humans)
        {
            UnityEngine.Object.Destroy(h);
        }

        humans.Clear();

        var s = CreateSacrificeRequest(humans);
        Debug.Log(s.forbiddenObject + " " + s.requestedObject);

        for (int i = 0; i <= 8; i++)
        {
            for (int j = 0; j < 2; j++)
            {
                GameObject newHuman = SpawnRandomHuman(i, j);
                humans.Add(newHuman);
                if (!IsSolvable(humans, "R1", "H1", new[] { "L1", "L2" }))
                {
                    humans.Remove(newHuman);
                    UnityEngine.Object.Destroy(newHuman);
                    j--;
                }
            }
        }
    }


    SacrificeRequest CreateSacrificeRequest(List<GameObject> humans)
    {
        string[] possibleRequests = new[] { "L1", "L2", "R1", "R2", "H1", "H2" };

        string request = possibleRequests[UnityEngine.Random.Range(0, possibleRequests.Length)];

        string[] possibleForbid = Array.FindAll(possibleRequests, r => r[0] != request[0]);

        string forbid = possibleForbid[UnityEngine.Random.Range(0, possibleForbid.Length)];

        string[] solutions = Array.FindAll(possibleRequests, r => r[0] != request[0] && r[0] != forbid[0]);

        if (humans.Count == 0 || !IsSolvable(humans, request, forbid, solutions))
        {
            return new SacrificeRequest(request, forbid);
        }

        return CreateSacrificeRequest(humans);
    }

    bool IsSolvable(List<GameObject> humans, string required, string forbidden, string[] availableSolutions)
    {
        List<string> solutions = new List<string>(availableSolutions);
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

        if (solutions.Count == 0 )
        {
            return false;
        }

        foreach (GameObject h in possibleHumans)
        {
            Human human = h.GetComponent<Human>();

            if (human.rightHandId == required || human.leftHandId == required || human.headId == required)
            {
                return true;
            }
        }

        return false;
    }

    GameObject SpawnRandomHuman(int x, int y)
    {
        int leftHand = UnityEngine.Random.Range(0, 3);
        int rightHand = UnityEngine.Random.Range(0, 3);
        int head = UnityEngine.Random.Range(0, 3);

        if (leftHand + rightHand + head == 0)
        {
            return SpawnRandomHuman(x, y);
        }

        if (leftHand > 0 && rightHand > 0 && head > 0)
        {
            return SpawnRandomHuman(x, y);
        }

        return SpawnHuman(new Vector2(-98 + x * 25, -50 + y * 43), leftHand, rightHand, head);
    }

    GameObject SpawnHuman(Vector2 position, int leftHand, int rightHand, int head) {
        GameObject crowd = GameObject.Find("/Crowd");
        GameObject music = GameObject.Find("/loop");

        GameObject spotInstance = GameObject.Instantiate(spot);
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
        humanInstance.transform.position = new Vector2(position.x, -160 + position.y);

        spotInstance.transform.parent = crowd.transform;
        spotInstance.transform.localPosition = position;

        spotInstance.transform.parent = spotInstance.transform;
        spotInstance.GetComponent<Spot>().assignedHuman = humanInstance;

        return humanInstance;
	}
}
