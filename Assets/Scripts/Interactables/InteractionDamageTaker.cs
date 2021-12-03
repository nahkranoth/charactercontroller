using System;
using System.Collections;
using UnityEngine;

public class InteractionDamageTaker : MonoBehaviour
{
    public InteractionDetector interaction;
    public Action<int, PlayerToolActionType> OnInteraction;
    public Action OnInteractionFinished;
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
        if (damageRecovering) return;
        damageRecovering = true;
        audioController.PlaySound(damageSound);
        StartCoroutine(DamageFinished());
    }
    
    IEnumerator DamageFinished()
    {
        yield return new WaitForSeconds(damageRecoveryTime);
        damageRecovering = false;
        OnInteractionFinished?.Invoke();
    }
    
}
