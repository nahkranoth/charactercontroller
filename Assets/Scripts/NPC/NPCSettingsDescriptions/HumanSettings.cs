using System;
using UnityEngine;

[CreateAssetMenu(fileName = "HumanSettings", menuName = "Custom/HumanSettings", order = 1)]
public class HumanSettings : INPCSettings
{

    public bool isShopKeeper = false;
    public bool isHotelOwner = false;
    
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
        return typeof(HumanStateNetwork);
    }
}