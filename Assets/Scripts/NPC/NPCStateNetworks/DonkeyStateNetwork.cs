using System.Collections.Generic;
using UnityEngine;
using Object = System.Object;

public class DonkeyStateNetwork:INPCStateNetwork
{
    private DonkeySettings settings;
    private NPCController parent;
    
    private MessageController messageController;
    private DeepstorageEntityInventory deepStorage;

    public Dictionary<string, AbstractNPCState> GetStateNetwork(NPCController _parent, Object rawSettings)
    {
        settings = rawSettings as DonkeySettings;
        parent = _parent;
        messageController = WorldGraph.Retrieve(typeof(MessageController)) as MessageController;
        deepStorage = WorldGraph.Retrieve(typeof(DeepstorageEntityInventory)) as DeepstorageEntityInventory;
        var dict = new Dictionary<string, AbstractNPCState>()
        {
            {"idle", new DonkeyIdleState()},
            {"follow", new DonkeyFollowState(settings)},
            {"flee", new DonkeyFleeState(settings)},
            {"die", new DonkeyDieState()},
        };
        
        foreach (var abstractEnemyState in dict)
        {
            abstractEnemyState.Value.Init(parent);
        }

        return dict;
    }

    public void OnTriggerByPlayer(PlayerToolActionType type)
    {
        if (type == PlayerToolActionType.Apply) deepStorage.Show(parent.inventory.storage, parent.myNpcHealth.Amount);
    }

    public string GetStartNode()
    {
        return "idle";
    }

    public string GetDamagedNode()
    {
        return "flee";
    }

    public string GetDamageFinishedNode()
    {
        return "flee";
    }

    public string GetDieNode()
    {
        return "die";
    }
}