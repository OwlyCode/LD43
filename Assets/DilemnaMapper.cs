using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DilemnaMapper
{
    public static SacrificeRequest CreateSacrificeRequest(List<GameObject> humans, int count = 0)
    {
        return Random.Range(1, 3) == 1 ? InOutDilemna.CreateSacrificeRequest(humans, count) : AmountDilemna.CreateSacrificeRequest(humans, count);
    }

    public static List<string> GetPossibleSolutions(List<GameObject> humans, SacrificeRequest request)
    {
        return request.requestType == SacrificeRequest.TYPE_IN_OUT ? InOutDilemna.GetPossibleSolutions(humans, request) : AmountDilemna.GetPossibleSolutions(humans, request);
    }

    public static bool IsSolved(List<GameObject> humans, SacrificeRequest request, string proposedSolution)
    {
        return request.requestType == SacrificeRequest.TYPE_IN_OUT ? InOutDilemna.IsSolved(humans, request, proposedSolution) : AmountDilemna.IsSolved(humans, request, proposedSolution);
    }

    public static bool IsSolvable(List<GameObject> humans, SacrificeRequest request)
    {
        return request.requestType == SacrificeRequest.TYPE_IN_OUT ? InOutDilemna.IsSolvable(humans, request) : AmountDilemna.IsSolvable(humans, request);
    }
}
