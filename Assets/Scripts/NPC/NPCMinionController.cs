using UnityEngine;

public class NPCMinionController:NPCController, ITargetableByEnemy
{
    public Transform GetTransform()
    {
        return transform;
    }
}
