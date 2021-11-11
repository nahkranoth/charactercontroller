using System;
using UnityEngine;

public class NPCHealth:MonoBehaviour
{
    private int amount;
    private int maxAmount;
    private bool alive = true;
    
    public Action Die;
    public Action<int> OnChange;

    public void Modify(int change)
    {
        amount += change;
        if (amount <= 0)
        {
            alive = false;
            Die?.Invoke();
        }
        if (amount > maxAmount) amount = maxAmount;
        if (alive) OnChange?.Invoke(amount);
    }

    public bool IsDead()
    {
        return !alive;
    }

    public void Set(int set)
    {
        amount = set;
        maxAmount = set;
        alive = true;
        if (amount <= 0) alive = false;
    }
    
}
