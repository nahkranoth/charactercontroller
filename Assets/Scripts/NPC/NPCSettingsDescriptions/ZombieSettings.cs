using System;
using UnityEngine;

[CreateAssetMenu(fileName = "ZombieSettings", menuName = "Custom/ZombieSettings", order = 1)]
public class ZombieSettings : INPCSettings
{
    public float knockbackAmount = .2f;

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
