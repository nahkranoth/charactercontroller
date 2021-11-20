using System;
using UnityEngine;

[Serializable]
public class PlayerStatus
{
    public int maxHealth = 50;
    public int health = 50;
    
    public float hunger = 1f;
    public float thirst = 1f;
    public float sleep = 1f;
    
    public float walkSpeed = 14f;
    public float runSpeed = 20f;
    public float chargeSpeed = 1f;
    public float chargeWalkSpeed = 3f;
    public float dodgeRollForce = 4f;

    public int money;
    
    public bool alive;

    public EntityInventory inventory;

    public int currentStep;
    public int currentLowStep;

    public Vector3 position;
}
