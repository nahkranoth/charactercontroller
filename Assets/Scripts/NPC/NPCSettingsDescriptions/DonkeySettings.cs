using System;
using UnityEngine;

[CreateAssetMenu(fileName = "DonkeySettings", menuName = "Custom/DonkeySettings", order = 1)]
public class DonkeySettings : INPCSettings
{

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
        return typeof(DonkeyStateNetwork);
    }
}