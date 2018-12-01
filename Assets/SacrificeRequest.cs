using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SacrificeRequest
{
    public readonly string requestedObject;
    public readonly string forbiddenObject;
    public readonly string[] solutions;

    public SacrificeRequest(string requestedObject, string forbiddenObject, string[] solutions)
    {
        this.requestedObject = requestedObject;
        this.forbiddenObject = forbiddenObject;
        this.solutions = solutions;
    }
}
