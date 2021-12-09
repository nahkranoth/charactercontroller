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
        get{return status.bodyState.health;}
        set{status.bodyState.health=value;}
    }
    
    public float Hunger
    {
        get{return status.bodyState.hunger;}
        set{status.bodyState.hunger=value;}
    }
    
    public float Thirst
    {
        get{return status.bodyState.thirst;}
        set{status.bodyState.thirst=value;}
    }
    
    public float Sleep
    {
        get{return status.bodyState.sleep;}
        set{status.bodyState.sleep=value;}
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
            var sum = result.Sum(x => x.bodySettings.walkSpeed);
            return sum + status.bodyState.walkSpeed;
        }
    }
    
    public int MaxHealth
    {
        get
        {
            var result = status.wearing.storage;
            var sum = result.Sum(x => x.bodySettings.maxHealth);
            return sum + status.bodyState.maxHealth;
        }
    }

    public float RunSpeed
    {
        get
        {
            var result = status.wearing.storage;
            var sum = result.Sum(x => x.bodySettings.runSpeed);
            return sum + status.bodyState.runSpeed;
        }
    }

    public float ChargeTime
    {
        get
        {
            var result = status.wearing.storage;
            var sum = result.Sum(x => x.bodySettings.chargeTime);
            return Mathf.Max(0.1f, status.bodyState.chargeTime + sum);
        }
    }

    public float DodgeRollForce
    {
        get
        {
            var result = status.wearing.storage;
            var sum = result.Sum(x => x.bodySettings.dodgeRollForce);
            return status.bodyState.dodgeRollForce + sum;
        }
    }

    public float MaxCarryWeight
    {
        get
        {
            var result = status.wearing.storage;
            var sum = result.Sum(x => x.bodySettings.maxCarryWeight);
            return status.bodyState.maxCarryWeight + sum;
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