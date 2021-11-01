using System;
using UnityEngine;

[CreateAssetMenu(fileName = "DonkeySettings", menuName = "Custom/DonkeySettings", order = 1)]
public class DonkeySettings : INPCSettings
{

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
        return typeof(DonkeyStateNetwork);
    }
}