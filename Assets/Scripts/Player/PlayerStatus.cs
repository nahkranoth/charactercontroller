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

    private bool alive;
    
    public PlayerStatus DeepCopy()
    {
        return MemberwiseClone() as PlayerStatus;
    }
    
    public Action Die;
    public Action<int> OnChange;

    public int CurrentHealth
    {
        get { return health; }
    }
    
    public int MaxHealth
    {
        get { return maxHealth; }
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
