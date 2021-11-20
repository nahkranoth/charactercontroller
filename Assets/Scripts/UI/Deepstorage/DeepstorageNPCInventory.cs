using System;
using UnityEngine;

public class DeepstorageNPCInventory : AbstractDeepStorageScreen
{
    
    public GameObject playerInventoryGrid;
    
    private void Awake()
    {
        WorldGraph.Subscribe(this, typeof(DeepstorageNPCInventory));
    }

    protected override void AfterStart()
    {
        input.OpenDeepStorageAsPlayer -= Hide;
        input.OpenDeepStorageAsPlayer += Hide;
    }

    public void ToggleShow(EntityInventory inventory)
    {
        if (mainPanel.activeSelf)
        {
            Hide();
            return;
        }
        Show(inventory);
    }

    public void Show(EntityInventory inventory)
    {
        if(mainPanel.activeSelf) return;
        activeInventory = inventory;
        infoPanel.info.text = "NPC inventory";
        input.BlockExcept(InputType.OpenInventory);
        mainPanel.SetActive(true);
        InstantiateItems(activeInventory, OnSelectItem, inventoryGrid.transform);
        InstantiateItems(player.Inventory, OnSelectItem, playerInventoryGrid.transform);
    }

    protected override void OnSelectItem(Item _item)
    {
        infoPanel.OnInfo(new DeepStorageInfoData()
        {
            item = _item,
            onFirstAction = new DeepStorageInfoAction
            {
                name = "Destroy",
                action = (item) =>
                {
                    player.Inventory.RemoveByItem(_item);
                    RerenderInventory();
                }
            }
        });
    }
  
}
