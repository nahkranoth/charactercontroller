using System;
using UnityEngine;

[CreateAssetMenu(fileName = "RabiteSettings", menuName = "Custom/RabiteSettings", order = 1)]
public class RabiteSettings : INPCSettings
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
        return typeof(RabiteStateNetwork);
    }
}