using System.Collections.Generic;
using UnityEngine;
using Object = System.Object;

public class RabiteStateNetwork:INPCStateNetwork
{
    private NPCController parentController;
    private RabiteSettings settings;
    public Dictionary<string, AbstractEnemyState> GetStateNetwork(NPCController _parentController, Object rawSettings)
    {
        parentController = _parentController;
        settings = rawSettings as RabiteSettings;
        
        var dict = new Dictionary<string, AbstractEnemyState>()
        {
            {"idle", new ZombieIdleState(settings)},
            {"roam", new ZombieRoamState(settings)},
            {"knockback", new KnockbackHitState(settings.knockbackAmount)},
            {"angry", new ZombieAngryState(settings)},
            {"die", new ZombieDieState()}
        };
        
        foreach (var abstractEnemyState in dict)
        {
            abstractEnemyState.Value.Init(parentController);
        }

        return dict;
    }

    public string GetStartNode()
    {
        return "idle";
    }

    public string GetDamagedNode()
    {
        return "knockback";
    }

    public string GetDamageFinishedNode()
    {
        return "angry";
    }

    public string GetDieNode()
    {
        return "die";
    }

    public void OnTriggerByPlayer(Collider2D collider, PlayerController player)
    {
        if (parentController.attacking && !parentController.damageTaker.damageRecovering)//TODO move to stateNetworks
        {
            player.Damage(settings.damage);
            parentController.attacking = false;
        }
    }
}