using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmountDilemna {

    public static SacrificeRequest CreateSacrificeRequest(List<GameObject> humans, int count = 0)
    {
        string[] possibleRequests = new[] { "L1", "L2", "R1", "R2", "H1", "H2" };

        string request = possibleRequests[UnityEngine.Random.Range(0, possibleRequests.Length)];
        int amount = UnityEngine.Random.Range(1, 10);
        SacrificeRequest sacrifice = new SacrificeRequest(request, amount, possibleRequests);
        List<string> solutions = GetPossibleSolutions(humans, sacrifice);

        if (count == 1000)
        {
            Debug.LogError("Could not create a valid sacrifice query");
            return null;
        }

        if (solutions.Count == 0)
        {
            return DilemnaMapper.CreateSacrificeRequest(humans, count + 1);
        }

        string solution = solutions[0];

        
        if (solution == sacrifice.requestedObject)
        {
            return DilemnaMapper.CreateSacrificeRequest(humans, count + 1);
        }

        string lure = "H1";

        switch (solution)
        {
            case "H1":
                lure = "H2";
                break;
            case "H2":
                lure = "H1";
                break;
            case "L1":
                lure = "L2";
                break;
            case "L2":
                lure = "L1";
                break;
            case "R1":
                lure = "R2";
                break;
            case "R2":
                lure = "R1";
                break;
        }

        string[] offeredSolutions = UnityEngine.Random.Range(0, 2) == 1 ? new[] { solution, lure } : new[] { lure, solution };

        return new SacrificeRequest(request, amount, offeredSolutions);
    }

    public static List<string> GetPossibleSolutions(List<GameObject> humans, SacrificeRequest request)
    {
        List<string> possibleSolutions = new List<string>();

        foreach (string s in request.solutions)
        {
            Dictionary<string, int> counts = new Dictionary<string, int>
            {
                {"H1", 0},
                {"H2", 0},
                {"L1", 0},
                {"L2", 0},
                {"R1", 0},
                {"R2", 0},
            };

            foreach (GameObject h in humans)
            {
                Human human = h.GetComponent<Human>();

                if (human.rightHandId == s || human.leftHandId == s || human.headId == s)
                {
                    counts[human.rightHandId]++;
                    counts[human.leftHandId]++;
                    counts[human.headId]++;
                }
            }

            if (counts[request.requestedObject] == request.requestedAmount)
            {
                possibleSolutions.Add(s);
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
