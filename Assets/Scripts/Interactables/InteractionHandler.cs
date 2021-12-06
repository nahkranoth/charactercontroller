using System;
using System.Collections;
using UnityEngine;

public class InteractionHandler : MonoBehaviour
{
    public InteractionDetector interaction;
    public Action<int, PlayerToolActionType> OnInteraction;
    public Action<PlayerToolActionType> OnInteractionFinished;
    public AudioController.AudioClipName damageSound;
    [HideInInspector] public bool damageRecovering = false;
    public float damageRecoveryTime = 2f;
    
    private AudioController audioController;
    
    private void Start()
    {
        interaction.OnInteraction -= TakeInteraction;
        interaction.OnInteraction += TakeInteraction;
        audioController = WorldGraph.Retrieve(typeof(AudioController)) as AudioController;
    }

    private void OnDestroy()
    {
        interaction.OnInteraction -= TakeInteraction;
    }

    public void TakeInteraction(int damage, PlayerToolActionType type)
    {
        OnInteraction?.Invoke(damage, type);
        StartCoroutine(InteractionFinished(type));
        damageRecovering = true;    
        audioController.PlaySound(damageSound);
    }
    
    IEnumerator InteractionFinished(PlayerToolActionType type)
    {
        yield return new WaitForSeconds(damageRecoveryTime);
        damageRecovering = false;
        OnInteractionFinished?.Invoke(type);
    }

}
