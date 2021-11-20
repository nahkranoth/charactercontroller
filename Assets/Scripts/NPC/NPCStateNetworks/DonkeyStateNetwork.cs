﻿using System.Collections.Generic;
using UnityEngine;
using Object = System.Object;

public class DonkeyStateNetwork:INPCStateNetwork
{
    private DonkeySettings settings;
    private NPCController parent;
    
    private MessageController messageController;

    public Dictionary<string, AbstractNPCState> GetStateNetwork(NPCController _parent, Object rawSettings)
    {
        settings = rawSettings as DonkeySettings;
        parent = _parent;
        messageController = WorldGraph.Retrieve(typeof(MessageController)) as MessageController;
        var dict = new Dictionary<string, AbstractNPCState>()
        {
            {"idle", new DonkeyIdleState()},
            {"follow", new DonkeyFollowState(settings)},
        };
        
        foreach (var abstractEnemyState in dict)
        {
            abstractEnemyState.Value.Init(parent);
        }

        return dict;
    }

    public void OnTriggerByPlayer()
    {
        messageController.QueMessage("Eey-haa");
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