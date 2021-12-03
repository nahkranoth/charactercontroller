using System;
using UnityEngine;

[Serializable]
public class EntityChance: IRandomWeight
{
    public GameObject entity;
    public float probability;
    public float Weight => probability;
}
