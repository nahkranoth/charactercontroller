using System.Collections.Generic;
using UnityEngine;
using Object = System.Object;

public class HumanStateNetwork:INPCStateNetwork
{
    private HumanSettings settings;
    private NPCController parent;
    public Dictionary<string, AbstractEnemyState> GetStateNetwork(NPCController _parent, Object rawSettings)
    {
        settings = rawSettings as HumanSettings;
        parent = _parent;
        
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

    public void OnTriggerByPlayer()
    {
        if (settings.isShopKeeper)
        {
            var ds = WorldGraph.Retrieve(typeof(DeepstorageShop)) as DeepstorageShop;
            ds.Show(parent.inventory.storage);
        }
        
        if (settings.isHotelOwner)
        {
            
        }
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