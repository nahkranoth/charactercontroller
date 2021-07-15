using System;
using UnityEngine;

[CreateAssetMenu(fileName = "ZombieSettings", menuName = "Custom/ZombieSettings", order = 1)]
public class ZombieSettings : INPCSettings
{
    public int health = 30;
    public int damage = 5;
    public float walkSpeed = 0.1f;
    public float roamChance = 0.9f;
    public float maxRoamDistance = 2f;
    public float knockbackAmount = .2f;
    public float strikingDistance = .5f;
    public float strikingChance = .999f;

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
        return typeof(ZombieStateNetwork);
    }
}
