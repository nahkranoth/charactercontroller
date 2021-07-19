using System;
using System.Collections.Generic;

public class RabiteStateNetwork:INPCStateNetwork
{
    public Dictionary<string, AbstractEnemyState> GetStateNetwork(NPCController parent, Object rawSettings)
    {
        RabiteSettings settings = rawSettings as RabiteSettings;
        
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
            abstractEnemyState.Value.Init(parent);
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
}