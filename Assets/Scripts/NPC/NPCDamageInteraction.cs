using System;
using UnityEngine;

public class NPCDamageInteraction:MonoBehaviour, IDamageTarget
{
    public NPCController npcController;

    public Action<int> OnDamage;
    private void Start()
    {
        npcController.handler.OnInteractionFinished -= DamageFinished;
        npcController.handler.OnInteractionFinished += DamageFinished;
        npcController.handler.OnInteraction -= OnInteraction;
        npcController.handler.OnInteraction += OnInteraction;
        npcController.OnDestroyMe -= Destroy;
        npcController.OnDestroyMe += Destroy;
    }
    
    private void OnInteraction(int amount, PlayerToolActionType type)
    {
        if (type == PlayerToolActionType.Slash && !npcController.settings.invincible)
        {
            Damage(amount);
        }
    }
    
    public void Damage(int amount)
    {
        npcController.attacking = false;
        npcController.myNpcHealth.Modify(-amount);
        OnDamage.Invoke(amount);
        if (npcController.myNpcHealth.IsDead())
        {
            Die();
            return;
        }
        npcController.SetState(npcController.stateNetwork.GetDamagedNode());
        npcController.animatorController.Damage();
    }
    
    private void DamageFinished()
    {
        if(!npcController.myNpcHealth.IsDead()) npcController.SetState(npcController.stateNetwork.GetDamageFinishedNode());
        npcController.triggerOccupied = false;
    }

    private void Die()
    {
        StopAllCoroutines();
        npcController.SetState(npcController.stateNetwork.GetDieNode());
        Destroy(npcController.handler);
    }

    public void Destroy()
    {
        npcController.handler.OnInteractionFinished -= DamageFinished;
        npcController.OnDestroyMe -= Destroy;
        if(npcController.dropPool?.collection.Count > 0) npcController.metaEntity.entityPlacer.GenerateCollectable(npcController.dropPool.GetRandom(), transform.localPosition);
        Destroy(gameObject);
    }

}
