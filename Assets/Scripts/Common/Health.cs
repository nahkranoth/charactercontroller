using System;
using UnityEngine;

public class Health:MonoBehaviour
{
    private int amount;
    public Action Die;
    
    public void Modify(int change)
    {
        amount += change;
        if(amount <= 0) Die?.Invoke();
    }

    public bool IsDeath()
    {
        return amount <= 0;
    }

    public void Set(int set)
    {
        amount = set;
    }
}
