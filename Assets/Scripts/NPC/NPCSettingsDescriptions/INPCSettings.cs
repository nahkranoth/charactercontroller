using System;
using UnityEngine;

public abstract class INPCSettings:ScriptableObject
{
    public abstract int GetHealth();
    public abstract int GetDamage();

    public abstract Type GetStateNetworkType();

}
