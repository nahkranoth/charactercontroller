using System;
using UnityEngine;

public class InteractionDetector : MonoBehaviour
{
    private PlayerController player;
    public Action<int> OnInteraction;
    private void Start()
    {
        player = WorldGraph.Retrieve(typeof(PlayerController)) as PlayerController;
        player.attackController.OnToolHitSomething -= OnPossibleInteraction;
        player.attackController.OnToolHitSomething += OnPossibleInteraction;
    }

    private void OnDestroy()
    {
        player.attackController.OnToolHitSomething -= OnPossibleInteraction;
    }

    private void OnPossibleInteraction(Collider2D other, Item tool)
    {
        InteractionDetector target = other.GetComponent<InteractionDetector>();
        if (target == this)
        {
            OnInteraction?.Invoke(player.attackController.CurrentDamage());
        }
    }
}
