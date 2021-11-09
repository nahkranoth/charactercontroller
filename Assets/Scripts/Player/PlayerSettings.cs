using System;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerSettings", menuName = "Custom/PlayerSettings", order = 1)]
[Serializable]
public class PlayerSettings : ScriptableObject
{
    public int health = 50;
    public float walkSpeed = 14f;
    public float runSpeed = 20f;
    public float chargeSpeed = 1f;
    public float chargeWalkSpeed = 3f;
    public float dodgeRollForce = 4f;
}
