using System;
using UnityEngine;

public abstract class INPCSettings:ScriptableObject
{
    public int health = 30;
    public int damage = 5;
    public float roamWalkSpeed = 0.1f;
    public float attackWalkSpeed = 0.4f;
    public float strikeDistance = 0.3f;
    public float strikeDelayTime = 4f;

    public Vector2Int roamChance = new Vector2Int(1,20);
    public float detectDistance = 1f;
    public float loseDistance = 1.4f;
    public int maxRoam = 10;
    
    public abstract int GetHealth();
    public abstract int GetDamage();

    public abstract Type GetStateNetworkType();

}
