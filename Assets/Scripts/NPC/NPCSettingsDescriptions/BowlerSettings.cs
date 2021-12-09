using System;
using UnityEngine;

[CreateAssetMenu(fileName = "BowlerSettings", menuName = "Custom/BowlerSettings", order = 1)]
public class BowlerSettings : INPCSettings
{
    public float knockbackAmount = .2f;
    
    public override int GetHealth()
    {
        return bodyState.health;
    }

    public override int GetDamage()
    {
        return attackDamage;
    }

    public override Type GetStateNetworkType()
    {
        return typeof(BowlerStateNetwork);
    }
}