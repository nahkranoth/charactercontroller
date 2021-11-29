using System;

[Serializable]
public class PlayerModifiers
{
    public int maxHealth;
    public float walkSpeed;
    public float runSpeed;
    public float chargeSpeed;
    public float chargeWalkSpeed;
    public float dodgeRollForce;
    public float maxCarryWeight;
    
    public void AddModifier(PlayerModifiers other)
    {
        maxHealth += other.maxHealth;
        walkSpeed += other.walkSpeed;
        runSpeed += other.runSpeed;
        chargeSpeed += other.chargeSpeed;
        chargeWalkSpeed += other.chargeWalkSpeed;
        dodgeRollForce += other.dodgeRollForce;
        maxCarryWeight += other.maxCarryWeight;
    }
    
    public void SubtractModifier(PlayerModifiers other)
    {
        maxHealth -= other.maxHealth;
        walkSpeed -= other.walkSpeed;
        runSpeed -= other.runSpeed;
        chargeSpeed -= other.chargeSpeed;
        chargeWalkSpeed -= other.chargeWalkSpeed;
        dodgeRollForce -= other.dodgeRollForce;
        maxCarryWeight -= other.maxCarryWeight;
    }
}
