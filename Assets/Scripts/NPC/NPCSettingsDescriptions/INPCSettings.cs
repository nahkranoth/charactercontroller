using System;
using UnityEngine;

public abstract class INPCSettings:ScriptableObject
{
    public EntityBodyState bodyState;
    
    public int attackDamage = 5;
    public float attackSpeed = 1.4f;
    public float strikeDistance = 0.3f;

    public Vector2Int roamChance = new Vector2Int(1,20);
    public float detectDistance = 1f;
    public float loseDistance = 1.4f;
    public int maxRoam = 10;
    
    public bool distanceCulling = true;
    
    public abstract int GetHealth();
    public abstract int GetDamage();

    public abstract Type GetStateNetworkType();

    public float WalkSpeed => bodyState.walkSpeed;
    public float RunSpeed => bodyState.runSpeed;
    public float ChargeTime => bodyState.chargeTime;
    public bool Invincible => bodyState.invincible;


}
