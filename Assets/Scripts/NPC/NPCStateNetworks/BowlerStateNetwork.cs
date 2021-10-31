using System.Collections.Generic;
using UnityEngine;
using Object = System.Object;

public class BowlerStateNetwork:INPCStateNetwork
{
    private NPCController parentController;
    private BowlerSettings settings;
    public Dictionary<string, AbstractEnemyState> GetStateNetwork(NPCController _parentController, Object rawSettings)
    {
        parentController = _parentController;
        settings = rawSettings as BowlerSettings;
        
        var dict = new Dictionary<string, AbstractEnemyState>()
        {
            {"idle", new ZombieIdleState(settings)},
            {"roam", new BowlerRoamState(settings)},
            {"knockback", new KnockbackHitState(settings.knockbackAmount)},
            {"angry", new BowlerAngryState(settings)},
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

    public void OnTriggerByPlayer()
    {
        
    }
}