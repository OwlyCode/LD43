using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InOutDilemna
{
    public static SacrificeRequest CreateSacrificeRequest(List<GameObject> humans, int count = 0)
    {
        string[] possibleRequests = new[] { "L1", "L2", "R1", "R2", "H1", "H2" };

        string request = possibleRequests[UnityEngine.Random.Range(0, possibleRequests.Length)];

        string[] possibleForbid = Array.FindAll(possibleRequests, r => r[0] != request[0]);

        string forbid = possibleForbid[UnityEngine.Random.Range(0, possibleForbid.Length)];

        string[] solutions = Array.FindAll(possibleRequests, r => r[0] != request[0] && r[0] != forbid[0]);

        SacrificeRequest sacrifice = new SacrificeRequest(request, forbid, solutions);

        if (humans.Count == 0 || DilemnaMapper.IsSolvable(humans, sacrifice))
        {
            return sacrifice;
        }

        if (count == 1000)
        {
            Debug.LogError("Could not create a valid sacrifice query");
            return null;
        }

        return DilemnaMapper.CreateSacrificeRequest(humans, count + 1); //recursion
    }

    public static List<string> GetPossibleSolutions(List<GameObject> humans, SacrificeRequest request)
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

    public static bool IsSolved(List<GameObject> humans, SacrificeRequest request, string proposedSolution)
    {
        return GetPossibleSolutions(humans, request).IndexOf(proposedSolution) != -1;
    }

    public static bool IsSolvable(List<GameObject> humans, SacrificeRequest request)
    {
        List<string> solutions = GetPossibleSolutions(humans, request);

        return solutions.Count != 0;
    }
}
