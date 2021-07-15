using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerSettings", menuName = "Custom/PlayerSettings", order = 1)]
public class PlayerSettings : ScriptableObject
{
    public float walkSpeed = 14f;
    public float chargeWalkSpeed = 3f;
}
