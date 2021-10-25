using System.Collections.Generic;
using UnityEngine;
using Object = System.Object;

public class HumanStateNetwork:INPCStateNetwork
{
    public Dictionary<string, AbstractEnemyState> GetStateNetwork(NPCController parent, Object rawSettings)
    {
        HumanSettings settings = rawSettings as HumanSettings;
        
        var dict = new Dictionary<string, AbstractEnemyState>()
        {
            {"idle", new HumanIdleState()}
        };
        
        foreach (var abstractEnemyState in dict)
        {
            abstractEnemyState.Value.Init(parent);
        }

        return dict;
    }

    public void OnTriggerByPlayer(Collider2D collider, PlayerController player)
    {
        Debug.Log("Human Triggered");
    }

    public string GetStartNode()
    {
        return "idle";
    }

    public string GetDamagedNode()
    {
        return "idle";
    }

    public string GetDamageFinishedNode()
    {
        return "idle";
    }

    public string GetDieNode()
    {
        return "idle";
    }
}