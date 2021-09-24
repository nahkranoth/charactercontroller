using System;
using UnityEngine;

public class InteractionDetector : MonoBehaviour
{
    private PlayerController player;
    public Action<int> OnInteraction;
    private void Start()
    {
        player = WorldGraph.Retrieve(typeof(PlayerController)) as PlayerController;
        player.attackController.OnWeaponHitSomething -= OnPossibleInteraction;
        player.attackController.OnWeaponHitSomething += OnPossibleInteraction;
    }

    private void OnDestroy()
    {
        player.attackController.OnWeaponHitSomething -= OnPossibleInteraction;
    }

    private void OnPossibleInteraction(Collider2D collider, int force)
    {
        InteractionDetector target = collider.GetComponent<InteractionDetector>();
        if (target == this)
        {
            OnInteraction?.Invoke(force);
        }
    }
}
