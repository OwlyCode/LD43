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
    public GameObject lightning;

    List<GameObject> humans;

    private SacrificeRequest currentSacrifice;

    private void Start()
    {
        humans = new List<GameObject>();
        StartCoroutine(NextWave());
    }

    private void Update()
    {

    }

    IEnumerator NextWave()
    {
        yield return new WaitForSeconds(3.0f);

        Generate();
    }

    void Generate()
    {
        humans.Clear();

        currentSacrifice = CreateSacrificeRequest(humans);
        GameObject.Find("/God").GetComponent<GodBehavior>().RequestSacrifice();

        IconFinder finder = GetComponent<IconFinder>();
        GameObject requestedIcon = UnityEngine.Object.Instantiate(finder.GetIcon(currentSacrifice.requestedObject));
        GameObject forbiddenIcon = UnityEngine.Object.Instantiate(finder.GetIcon(currentSacrifice.forbiddenObject));

        GameObject requestedSlot = GameObject.Find("/Bubble/RequestedSlot");
        GameObject forbiddenSlot = GameObject.Find("/Bubble/ForbiddenSlot");

        foreach (Transform child in requestedSlot.transform)
        {
            GameObject.Destroy(child.gameObject);
        }

        foreach (Transform child in forbiddenSlot.transform)
        {
            GameObject.Destroy(child.gameObject);
        }

        requestedIcon.transform.parent = requestedSlot.transform;
        requestedIcon.transform.localPosition = Vector2.zero;
        forbiddenIcon.transform.parent = forbiddenSlot.transform;
        forbiddenIcon.transform.localPosition = Vector2.zero;

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

        leftPillar.GetComponent<Pillar>().currentSolution = currentSacrifice.solutions[0];
        rightPillar.GetComponent<Pillar>().currentSolution = currentSacrifice.solutions[1];

        GameObject leftSolutionIcon = UnityEngine.Object.Instantiate(finder.GetIcon(currentSacrifice.solutions[0]));
        GameObject rightSolutionIcon = UnityEngine.Object.Instantiate(finder.GetIcon(currentSacrifice.solutions[1]));

        leftSolutionIcon.transform.parent = leftPillar.transform;
        rightSolutionIcon.transform.parent = rightPillar.transform;

        leftSolutionIcon.GetComponent<SpriteRenderer>().sortingLayerName = leftPillar.GetComponent<SpriteRenderer>().sortingLayerName;
        leftSolutionIcon.transform.localPosition = new Vector2(0, 20f);
        rightSolutionIcon.GetComponent<SpriteRenderer>().sortingLayerName = rightPillar.GetComponent<SpriteRenderer>().sortingLayerName;
        rightSolutionIcon.transform.localPosition = new Vector2(0, 20f);

        foreach(Transform spotTransform in GameObject.Find("/Crowd").transform) {
            GameObject spot = spotTransform.gameObject;
            Spot spotComponent = spot.GetComponent<Spot>();

            if (!spotComponent.assignedHuman)
            {
                GameObject newHuman = SpawnRandomHuman(spotTransform.position);
                humans.Add(newHuman);
                while (!IsSolvable(humans, currentSacrifice))
                {
                    humans.Remove(newHuman);
                    UnityEngine.Object.Destroy(newHuman);
                    newHuman = SpawnRandomHuman(spotTransform.position);
                    humans.Add(newHuman);
                }

                spotComponent.assignedHuman = newHuman;
            } else {
                humans.Add(spotComponent.assignedHuman);
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

        SacrificeRequest sacrifice = new SacrificeRequest(request, forbid, solutions);

        if (humans.Count == 0 || IsSolvable(humans, sacrifice))
        {
            return sacrifice;
        }

        return CreateSacrificeRequest(humans);
    }

    List<string> GetPossibleSolutions(List<GameObject> humans, SacrificeRequest request)
    {
        List<string> solutions = new List<string>(request.solutions);
        List<GameObject> possibleHumans = new List<GameObject>();

        foreach (GameObject h in humans)
        {
            Human human = h.GetComponent<Human>();

            if (human.rightHandId == request.forbiddenObject || human.leftHandId == request.forbiddenObject || human.headId == request.forbiddenObject)
            {
                int rightPos = solutions.IndexOf(human.rightHandId);
                if (rightPos != -1)
                {
                    solutions.RemoveAt(rightPos);
                }

                int leftPos = solutions.IndexOf(human.leftHandId);
                if (leftPos != -1)
                {
                    solutions.RemoveAt(leftPos);
                }

                int headPos = solutions.IndexOf(human.headId);
                if (headPos != -1)
                {
                    solutions.RemoveAt(headPos);
                }
            }
            else
            {
                possibleHumans.Add(h);
            }
        }

        List<string> possibleSolutions = new List<string>();

        foreach (GameObject h in possibleHumans)
        {
            Human human = h.GetComponent<Human>();

            foreach (string s in solutions)
            {
                if (
                    (human.rightHandId == request.requestedObject || human.leftHandId == request.requestedObject || human.headId == request.requestedObject) &&
                    (human.rightHandId == s || human.leftHandId == s || human.headId == s) &&
                    possibleSolutions.IndexOf(s) == -1
                )
                {
                    possibleSolutions.Add(s);
                }
            }
        }

        return possibleSolutions;
    }

    public void Solve(string solution)
    {
        if (IsSolved(humans, currentSacrifice, solution))
        {
            GameObject.Find("/God").GetComponent<GodBehavior>().Soften();
        } else
        {
            GameObject.Find("/God").GetComponent<GodBehavior>().Anger();
        }

        foreach (GameObject h in humans)
        {
            Human human = h.GetComponent<Human>();

            if (human.HasAccessory(solution))
            {
                GameObject lightningInstance = Instantiate(lightning);
                lightningInstance.transform.position = h.transform.position;
                lightningInstance.GetComponent<Lightning>().target = h;
            }
        }

        GameObject.Find("Sound/Thunder").GetComponent<AudioSource>().Play();
        TriggerNextWave();
    }

    public void TriggerNextWave()
    {
        StartCoroutine(NextWave());
    }

    public bool IsSolved(List<GameObject> humans, SacrificeRequest request, string proposedSolution)
    {
        return GetPossibleSolutions(humans, request).IndexOf(proposedSolution) != -1;
    }

    bool IsSolvable(List<GameObject> humans, SacrificeRequest request)
    {
        List<string> solutions = GetPossibleSolutions(humans, request);

        return solutions.Count != 0;
    }

    GameObject SpawnRandomHuman(Vector2 position)
    {
        int leftHand = UnityEngine.Random.Range(1, 3);
        int rightHand = UnityEngine.Random.Range(1, 3);
        int head = UnityEngine.Random.Range(1, 3);

        return SpawnHuman(position, leftHand, rightHand, head);
    }

    GameObject SpawnHuman(Vector2 position, int leftHand, int rightHand, int head) {
        GameObject crowd = GameObject.Find("/Crowd");
        GameObject music = GameObject.Find("/Sound/Music");

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
        humanInstance.transform.position = new Vector2(position.x, -80 + position.y);

        return humanInstance;
	}
}
