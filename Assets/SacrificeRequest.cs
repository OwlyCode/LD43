using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SacrificeRequest
{
    public readonly string requestType;
    public readonly string requestedObject;
    public readonly int requestedAmount;
    public readonly string forbiddenObject;
    public readonly string[] solutions;

    public const string TYPE_IN_OUT = "inOut";
    public const string TYPE_AMOUNT = "amount";

    public SacrificeRequest(string requestedObject, string forbiddenObject, string[] solutions)
    {
        this.requestType = TYPE_IN_OUT;
        this.requestedObject = requestedObject;
        this.forbiddenObject = forbiddenObject;
        this.solutions = solutions;
    }

    public SacrificeRequest(string requestedObject, int askedAmount, string[] solutions)
    {
        this.requestType = TYPE_AMOUNT;
        this.requestedAmount = askedAmount;
        this.requestedObject = requestedObject;
        this.solutions = solutions;
    }
}
