using System;

[Serializable]
public class EntityBodyState
{
    public int health = 50;
    public int maxHealth;
    public float walkSpeed;
    public float runSpeed;
    public float chargeTime;
    public float dodgeRollForce;
    public float maxCarryWeight;
    public float hunger = 1f;
    public float thirst = 1f;
    public float sleep = 1f;
    public bool invincible = false;

}
