using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IconFinder : MonoBehaviour {

    public GameObject ironSpear;
    public GameObject goldenSpear;
    public GameObject redHelmet;
    public GameObject blueHelmet;
    public GameObject ironShield;
    public GameObject goldenShield;

    public GameObject GetIcon(string name)
    {
        switch(name)
        {
            case "H1":
                return redHelmet;
            case "H2":
                return blueHelmet;
            case "L1":
                return goldenShield;
            case "L2":
                return ironShield;
            case "R1":
                return goldenSpear;
            case "R2":
                return ironSpear;
        }

        return null;
    }
}
