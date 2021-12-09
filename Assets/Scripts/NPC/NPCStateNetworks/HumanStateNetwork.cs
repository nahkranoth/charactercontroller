using System;
using System.Collections.Generic;
using UnityEngine;
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

    public void OnTriggerByPlayer(PlayerToolActionType type)
    {
        if (type != PlayerToolActionType.Apply) return; //should make them angry
        if (settings.isShopKeeper)
        { //open inventory
            var ds = WorldGraph.Retrieve(typeof(DeepstorageShopInventory)) as DeepstorageShopInventory;
            ds.Show(parent.inventory.storage);
            return;
        }
        
        var playerController = WorldGraph.Retrieve(typeof(PlayerController)) as PlayerController;
        var worldTimeController = WorldGraph.Retrieve(typeof(WorldTimeController)) as WorldTimeController;
        
        if (settings.isHotelOwner)
        {
            //Give option to sleep - and save
            messageController.QueMessageQuestion("Do you want to sleep here? It's 30$",
                new List<(string, Action)> {("Yes", () =>
                {
                    Debug.Log("Sleep");
                    //TODO; do time thingy
                    playerController.statusController.ChangeMoney(-30);
                    playerController.statusController.SetHealth(playerController.statusController.MaxHealth);
                    playerController.statusController.Hunger = 1;
                    playerController.statusController.Thirst = 1;
                    playerController.statusController.Sleep = 1;
                    worldTimeController.SpeedToMorning();
                }), ("No", () =>
                {
                    messageController.QueMessage("Okay Bye!");
                })});
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