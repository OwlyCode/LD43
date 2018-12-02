using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    GameObject requestedSlot;
    GameObject counter;
    GameObject forbiddenSlot;

    private SacrificeRequest currentSacrifice;
    bool haltGame = false;

    private void Start()
    {
        humans = new List<GameObject>();
        StartCoroutine(NextWave());

        requestedSlot = GameObject.Find("/Bubble/RequestedSlot");
        counter = GameObject.Find("/Bubble/CounterSlot");
        forbiddenSlot = GameObject.Find("/Bubble/ForbiddenSlot");
    }

    IEnumerator NextWave()
    {
        yield return new WaitForSeconds(3.0f);

        if (!haltGame)
        {
            Generate();
        }

    }

    void Generate()
    {
        humans.Clear();

        foreach (Transform spotTransform in GameObject.Find("/Crowd").transform)
        {
            GameObject spot = spotTransform.gameObject;
            Spot spotComponent = spot.GetComponent<Spot>();

            if (spotComponent.assignedHuman)
            {
                humans.Add(spotComponent.assignedHuman);
            }
        }


        foreach (Transform spotTransform in GameObject.Find("/Crowd").transform)
        {
            GameObject spot = spotTransform.gameObject;
            Spot spotComponent = spot.GetComponent<Spot>();

            if (!spotComponent.assignedHuman)
            {
                GameObject newHuman = SpawnRandomHuman(spotTransform.position);
                humans.Add(newHuman);

                spotComponent.assignedHuman = newHuman;
            }
        }

        currentSacrifice = DilemnaMapper.CreateSacrificeRequest(humans);

        GameObject.Find("/God").GetComponent<GodBehavior>().RequestSacrifice();

        IconFinder finder = GetComponent<IconFinder>();

        if (currentSacrifice.requestType == SacrificeRequest.TYPE_AMOUNT)
        {
            GameObject requestedIcon = UnityEngine.Object.Instantiate(finder.GetIcon(currentSacrifice.requestedObject));

            forbiddenSlot.SetActive(false);
            counter.SetActive(true);

            counter.GetComponent<TextMesh>().text = "x" + currentSacrifice.requestedAmount;

            foreach (Transform child in requestedSlot.transform)
            {
                GameObject.Destroy(child.gameObject);
            }

            requestedIcon.transform.parent = requestedSlot.transform;
            requestedIcon.transform.localPosition = Vector2.zero;
        } else {
            GameObject requestedIcon = UnityEngine.Object.Instantiate(finder.GetIcon(currentSacrifice.requestedObject));
            GameObject forbiddenIcon = UnityEngine.Object.Instantiate(finder.GetIcon(currentSacrifice.forbiddenObject));

            forbiddenSlot.SetActive(true);
            counter.SetActive(false);

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
        }
 
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
        leftPillar.GetComponent<Pillar>().StartBlinking();
        rightPillar.GetComponent<Pillar>().StartBlinking();

        GameObject leftSolutionIcon = UnityEngine.Object.Instantiate(finder.GetIcon(currentSacrifice.solutions[0]));
        GameObject rightSolutionIcon = UnityEngine.Object.Instantiate(finder.GetIcon(currentSacrifice.solutions[1]));

        leftSolutionIcon.transform.parent = leftPillar.transform;
        rightSolutionIcon.transform.parent = rightPillar.transform;

        leftSolutionIcon.GetComponent<SpriteRenderer>().sortingLayerName = leftPillar.GetComponent<SpriteRenderer>().sortingLayerName;
        leftSolutionIcon.transform.localPosition = new Vector2(0, 20f);
        rightSolutionIcon.GetComponent<SpriteRenderer>().sortingLayerName = rightPillar.GetComponent<SpriteRenderer>().sortingLayerName;
        rightSolutionIcon.transform.localPosition = new Vector2(0, 20f);
    }
    
    public void Solve(string solution)
    {
        if (DilemnaMapper.IsSolved(humans, currentSacrifice, solution))
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

    GameObject SpawnRandomHuman(Vector2 position)
    {
        int leftHand = UnityEngine.Random.Range(1, 3);
        int rightHand = UnityEngine.Random.Range(1, 3);
        int head = UnityEngine.Random.Range(1, 3);

        return SpawnHuman(position, leftHand, rightHand, head);
    }

    GameObject SpawnHuman(Vector2 position, int leftHand, int rightHand, int head) {
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
        humanInstance.transform.position = new Vector2(position.x / 10f, -80 + position.y);
        humanInstance.transform.parent = GameObject.Find("/Misc").transform;

        return humanInstance;
	}

    public void Win()
    {
        haltGame = true;
        GameObject.Find("/Sound/Music").GetComponent<AudioSource>().Stop();
        GameObject.Find("/God").GetComponent<GodBehavior>().Leave();
        StartCoroutine(GoToScene("Victory", 5f, new Color(49f / 255, 77f / 255, 121f / 255)));
    }

    public void Loose()
    {
        haltGame = true;
        GameObject.Find("/Sound/Music").GetComponent<AudioSource>().Stop();
        GameObject.Find("/God").GetComponent<GodBehavior>().EndTheWorld();

        StartCoroutine(ThunderStorm());
    }

    IEnumerator GoToScene(string scene, float delay, Color fadingColor)
    {
        yield return new WaitForSeconds(delay);

        yield return Fading.Out(fadingColor, 0.5f);

        SceneManager.LoadScene(scene);
    }

    IEnumerator ThunderStorm()
    {
        AudioSource thunder = GameObject.Find("Sound/Thunder").GetComponent<AudioSource>();

        Transform misc = GameObject.Find("/Misc").transform;

        while (misc.childCount > 0)
        {
            for(int i = 0; i < 8 && i < misc.childCount; i++)
            {
                Transform target = misc.GetChild(i);
                GameObject lightningInstance = Instantiate(lightning);
                lightningInstance.transform.position = target.position;
                lightningInstance.GetComponent<Lightning>().target = target.gameObject;
            }

            thunder.Play();
            yield return new WaitForSeconds(.5f);
        }

        yield return GoToScene("Defeat", 1f, Color.black);
    }
}
