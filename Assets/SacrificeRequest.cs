using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SacrificeRequest
{
    public readonly string requestedObject;
    public readonly string forbiddenObject;

    public SacrificeRequest(string requestedObject, string forbiddenObject)
    {
        this.requestedObject = requestedObject;
        this.forbiddenObject = forbiddenObject;
    }
}
