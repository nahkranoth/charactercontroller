using System;
using System.Collections.Generic;
using Object = System.Object;

public class HumanStateNetwork:INPCStateNetwork
{
    private HumanSettings settings;
    private NPCController parent;

    private MessageController messageController;
    public Dictionary<string, AbstractNPCState> GetStateNetwork(NPCController _parent, Object rawSettings)
    {
        settings = rawSettings as HumanSettings;
        parent = _parent;
        
        messageController = WorldGraph.Retrieve(typeof(MessageController)) as MessageController;

        var dict = new Dictionary<string, AbstractNPCState>()
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
        { //open inventory
            var ds = WorldGraph.Retrieve(typeof(DeepstorageShopInventory)) as DeepstorageShopInventory;
            ds.Show(parent.inventory.storage);
            return;
        }
        
        if (settings.isHotelOwner)
        {
            //Give option to sleep - and save
            messageController.QueMessageQuestion("Do you want to sleep here?",
                new List<(string, Action)> {("Yes", null), ("No", null)});
            return;
        }
        
        messageController.QueMessage("Nice weather aint it?");
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