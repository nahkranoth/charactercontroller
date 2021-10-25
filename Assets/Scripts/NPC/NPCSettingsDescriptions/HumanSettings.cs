using System;
using UnityEngine;

[CreateAssetMenu(fileName = "HumanSettings", menuName = "Custom/HumanSettings", order = 1)]
public class HumanSettings : INPCSettings
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
        return typeof(HumanStateNetwork);
    }
}