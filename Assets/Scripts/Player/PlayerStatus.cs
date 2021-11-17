using System;

[Serializable]
public class PlayerStatus
{
    public int maxHealth = 50;
    public int health = 50;
    
    public float hunger = 1f;
    public float thirst = 1f;
    public float sleep = 1f;
    
    public float walkSpeed = 14f;
    public float runSpeed = 20f;
    public float chargeSpeed = 1f;
    public float chargeWalkSpeed = 3f;
    public float dodgeRollForce = 4f;

    public int money;
    
    private bool alive;

    public Action Die;
    public Action<int> OnChange;
    
    public EntityInventory inventory;
    
    public Action<int> OnMoneyChange;

    public int Money
    {
        get { return money; }
        set { money = value; }
    }

    public void ChangeMoney(int amount)
    {
        money += amount;
        Update();
    }

    public void Update()
    {
        OnMoneyChange?.Invoke(money);
        OnChange?.Invoke(health);
    }

    public int CurrentHealth
    {
        get { return health; }
        set { health = value; }
    }
    
    public int MaxHealth
    {
        get { return maxHealth; }
        set { maxHealth = value; }
    }
    
    public void ModifyHealth(int change)
    {
        health += change;
        if (health <= 0)
        {
            alive = false;
            Die?.Invoke();
        }
        if (health > maxHealth) health = maxHealth;
        if (alive) OnChange?.Invoke(health);
    }

    public void SetHealth(int set)
    {
        health = set;
        maxHealth = set;
        alive = true;
        if (health <= 0) alive = false;
    }

    public bool IsDead()
    {
        return !alive;
    }

}
