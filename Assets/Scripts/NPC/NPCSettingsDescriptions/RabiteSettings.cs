using System;
using UnityEngine;

[CreateAssetMenu(fileName = "RabiteSettings", menuName = "Custom/RabiteSettings", order = 1)]
public class RabiteSettings : INPCSettings
{
    public int health = 20;
    public int damage = 2;
    public float walkSpeed = 0.1f;
    public float roamChance = 0.01f;
    public float maxRoamDistance = 2f;
    public float knockbackAmount = .2f;
    public float attackSpeed = 12f;
    public float strikingDistance = .5f;
    public float strikingChance = .999f;
    public float loseTrackDistance = 1f;
    
    public override int GetHealth()
    {
        return health;
    }

    public override int GetDamage()
    {
        return damage;
    }

    public override Type GetStateNetworkType()
    {
        return typeof(RabiteStateNetwork);
    }
}