using UnityEngine;

public class NPCDamageInteraction:MonoBehaviour
{
    public NPCController npcController;

    private void Start()
    {
        npcController.damageTaker.OnInteractionFinished -= DamageFinished;
        npcController.damageTaker.OnInteractionFinished += DamageFinished;
        npcController.damageTaker.OnInteraction -= OnInteraction;
        npcController.damageTaker.OnInteraction += OnInteraction;
        npcController.OnDestroyMe -= Destroy;
        npcController.OnDestroyMe += Destroy;
    }
    
    private void OnInteraction(int amount, PlayerToolActionType type)
    {
        if (type == PlayerToolActionType.Slash && !npcController.settings.invincible)
        {
            Damage(amount, type);
        }
    }
    
    private void Damage(int amount, PlayerToolActionType type)
    {
        npcController.attacking = false;
        npcController.myNpcHealth.Modify(-amount);
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
        npcController.damageTaker.OnInteraction -= Damage;
        Destroy(npcController.damageTaker);
    }

    public void Destroy()
    {
        npcController.damageTaker.OnInteractionFinished -= DamageFinished;
        npcController.OnDestroyMe -= Destroy;
        if(npcController.dropPool?.collection.Count > 0) npcController.metaEntity.entityPlacer.GenerateCollectable(npcController.dropPool.GetRandom(), transform.localPosition);
        Destroy(gameObject);
    }
}
