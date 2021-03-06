using System;
using UnityEngine;

[Serializable]
public class PlayerStatus
{
    public SentientBodyState bodyState;
    public ActiveSentientBodyState activeBodyState;
    
    public int money;
    public bool alive;

    public EntityInventory inventory;
    public EntityInventory wearing;
    
    //level progress specific data
    public int currentLevelStep;
    public int currentLowLevelStep;
    public Vector3 position;

}
