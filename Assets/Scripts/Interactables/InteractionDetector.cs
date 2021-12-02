using System;
using UnityEngine;

public class InteractionDetector : MonoBehaviour
{
    private PlayerController player;
    public Action<int, PlayerToolActionType> OnInteraction;
    private void Start()
    {
        player = WorldGraph.Retrieve(typeof(PlayerController)) as PlayerController;
        player.toolController.OnToolHitSomething -= OnPossibleInteraction;
        player.toolController.OnToolHitSomething += OnPossibleInteraction;
    }

    private void OnDestroy()
    {
        player.toolController.OnToolHitSomething -= OnPossibleInteraction;
    }

    private void OnPossibleInteraction(Collider2D other, Item tool, PlayerToolActionType type)
    {
        InteractionDetector target = other.GetComponent<InteractionDetector>();
        if (target == this)
        {
            OnInteraction?.Invoke(player.toolController.CurrentDamage(), type);
        }
    }
}
