using System;
using System.Collections;
using UnityEngine;

public class InteractionDamageTaker : MonoBehaviour
{
    public InteractionDetector interaction;
    public Action<int> OnTakeDamage;
    public Action OnDamageFinished;
    public AudioController.AudioClipName damageSound;
    [HideInInspector] public bool damageRecovering = false;
    public float damageRecoveryTime = 2f;
    public DamageIndicator damageIndicator;
    
    private AudioController audioController;
    
    private void Start()
    {
        interaction.OnInteraction -= TakeDamage;
        interaction.OnInteraction += TakeDamage;
        audioController = WorldGraph.Retrieve(typeof(AudioController)) as AudioController;
    }

    private void OnDestroy()
    {
        interaction.OnInteraction -= TakeDamage;
    }

    public void TakeDamage(int damage)
    {
        if (damageRecovering) return;
        damageRecovering = true;
        damageIndicator.ShowDamage(damage);
        OnTakeDamage?.Invoke(damage);
        audioController.PlaySound(damageSound);
        StartCoroutine(DamageFinished());
    }
    
    IEnumerator DamageFinished()
    {
        yield return new WaitForSeconds(damageRecoveryTime);
        damageRecovering = false;
        OnDamageFinished?.Invoke();
    }
    
}
