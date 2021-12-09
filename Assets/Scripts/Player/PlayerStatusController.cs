using System;
using System.Linq;
using UnityEngine;

public class PlayerStatusController:MonoBehaviour
{
    public PlayerStatus status;
    
    public Action<int> OnMoneyChange;
    public Action<int> OnChangeHealth;
    public Action OnFullReset;

    private void Awake()
    {
        WorldGraph.Subscribe(this, typeof(PlayerStatusController));
    }

    public void ChangeMoney(int amount)
    {
        status.money += amount;
        StatusUpdate();
    }

    public void FullReset()
    {
        OnFullReset?.Invoke();
    }
    
    public void StatusUpdate()
    {
        OnMoneyChange?.Invoke(status.money);
        OnChangeHealth?.Invoke(CurrentHealth);
    }

    public int Money => status.money;

    public int CurrentHealth
    {
        get{return status.modifiers.health;}
        set{status.modifiers.health=value;}
    }
    
    public float Hunger
    {
        get{return status.modifiers.hunger;}
        set{status.modifiers.hunger=value;}
    }
    
    public float Thirst
    {
        get{return status.modifiers.thirst;}
        set{status.modifiers.thirst=value;}
    }
    
    public float Sleep
    {
        get{return status.modifiers.sleep;}
        set{status.modifiers.sleep=value;}
    }
    
    public float Armor
    {
        get
        {
            var result = status.wearing.storage;
            var sum = result.Sum(x => x.armor);
            return 1-Mathf.Min(1, sum);
        }
    }
    
    public float WalkSpeed
    {
        get
        {
            var result = status.wearing.storage;
            var sum = result.Sum(x => x.wearableModifier.walkSpeed);
            return sum + status.modifiers.walkSpeed;
        }
    }
    
    public int MaxHealth
    {
        get
        {
            var result = status.wearing.storage;
            var sum = result.Sum(x => x.wearableModifier.maxHealth);
            return sum + status.modifiers.maxHealth;
        }
    }

    public float RunSpeed
    {
        get
        {
            var result = status.wearing.storage;
            var sum = result.Sum(x => x.wearableModifier.runSpeed);
            return sum + status.modifiers.runSpeed;
        }
    }

    public float ChargeTime
    {
        get
        {
            var result = status.wearing.storage;
            var sum = result.Sum(x => x.wearableModifier.chargeTime);
            return Mathf.Max(0.1f, status.modifiers.chargeTime + sum);
        }
    }

    public float DodgeRollForce
    {
        get
        {
            var result = status.wearing.storage;
            var sum = result.Sum(x => x.wearableModifier.dodgeRollForce);
            return status.modifiers.dodgeRollForce + sum;
        }
    }

    public float MaxCarryWeight
    {
        get
        {
            var result = status.wearing.storage;
            var sum = result.Sum(x => x.wearableModifier.maxCarryWeight);
            return status.modifiers.maxCarryWeight + sum;
        }
    }
    
    public void ModifyHealth(int change)
    {
        if (change < 0)
        { //is damage
            change = (int) (change * Armor);
            // change = Math.Min(change, 0);
        }
        
        CurrentHealth += change;
        if (CurrentHealth <= 0)
        {
            status.alive = false;
        }
        if (CurrentHealth > MaxHealth) CurrentHealth = MaxHealth;
        if (status.alive) OnChangeHealth?.Invoke(CurrentHealth);
    }

    public void SetHealth(int set)
    {
        CurrentHealth = set;
        status.alive = true;
        if (CurrentHealth <= 0) status.alive = false;
    }

    public bool IsDead()
    {
        return !status.alive;
    }

    public void OverrideStatus(PlayerStatus overStatus)
    {
        status = Merger.CloneAndMerge(status, overStatus);
        FullReset();
        StatusUpdate();
    }

    public bool HasCarrySpace(float weight)
    {
        return MaxCarryWeight > status.inventory.TotalItemWeight() + weight;
    }
}