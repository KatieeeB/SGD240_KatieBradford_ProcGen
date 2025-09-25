using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class OreType
{
    public string name;
    public GameObject prefab;
    [Range(0, 100)] public int fillPercent;
    public int caSteps;
}

