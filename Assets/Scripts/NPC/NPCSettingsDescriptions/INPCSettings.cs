using System;
using UnityEngine;

public abstract class INPCSettings:ScriptableObject
{
    public int health = 30;
    public int damage = 5;
    public float roamWalkSpeed = 0.1f;
    public float attackWalkSpeed = 0.4f;
    public float strikeDistance = 0.3f;

    public Vector2 roamChance = new Vector2(0.5f,2);
    
    public int maxRoam = 10;
    
    public abstract int GetHealth();
    public abstract int GetDamage();

    public abstract Type GetStateNetworkType();

}
