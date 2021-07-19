using System;
using System.Collections.Generic;

public class ZombieStateNetwork:INPCStateNetwork
{
    public Dictionary<string, AbstractEnemyState> GetStateNetwork(NPCController parent, Object rawSettings)
    {
        ZombieSettings settings = rawSettings as ZombieSettings;//TODO Can now also be RabiteSettings FIX
        
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
