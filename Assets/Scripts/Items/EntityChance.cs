using System;
using UnityEngine;

[Serializable]
public class EntityChance: IRandomProbability
{
    public GameObject entity;
    public float probability;
    public float Probability => probability;
}
