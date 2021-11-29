using System;
using UnityEngine;

[Serializable]
public class PlayerStatus
{
    public int health = 50;

    public PlayerModifiers modifiers;
    
    public float hunger = 1f;
    public float thirst = 1f;
    public float sleep = 1f;

    public int money;
    
    public bool alive;

    public EntityInventory inventory;
    public EntityInventory wearing;
    
    public int currentStep;
    public int currentLowStep;
    
    public Vector3 position;

}
