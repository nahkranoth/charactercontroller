using System;

[Serializable]
public class EntityBodyState
{
    public int health;
    public int maxHealth;
    public float walkSpeed;
    public float runSpeed;
    public float chargeTime;
    public float dodgeRollForce;
    public float maxCarryWeight;
    public float hunger;
    public float thirst;
    public float sleep;
    public bool invincible = false;
}
