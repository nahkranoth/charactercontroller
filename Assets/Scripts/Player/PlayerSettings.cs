using UnityEngine;

[CreateAssetMenu(fileName = "PlayerSettings", menuName = "Custom/PlayerSettings", order = 1)]
public class PlayerSettings : ScriptableObject
{
    public int startHealth = 50;
    public float walkSpeed = 14f;
    public float chargeWalkSpeed = 3f;
}
