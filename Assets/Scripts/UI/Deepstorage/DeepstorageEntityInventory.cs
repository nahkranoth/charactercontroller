using System;
using UnityEngine;

public class DeepstorageEntityInventory : AbstractDeepStorageScreen
{
    public GameObject playerInventoryGrid;
    
    private void Awake()
    {
        WorldGraph.Subscribe(this, typeof(DeepstorageEntityInventory));
    }
    public void Show(EntityInventory inventory, int health)
    {
        if(mainPanel.activeSelf) return;
        activeInventory = inventory;
        infoPanel.info.text = $"NPC inventory \n Health: {health}";
        mainPanel.SetActive(true);
        input.ChangeScheme("UI");
        input.OnCloseUI += Hide;
        RerenderInventory();
    }

    protected override void AfterStart()
    {
    }

    private void RerenderInventory()
    {
        DestroyItems(OnSelectItem, inventoryGrid.transform);
        DestroyItems(OnSelectItem, playerInventoryGrid.transform);
        InstantiateItems(activeInventory.storage, OnSelectItem, inventoryGrid.transform);
        InstantiateItems(player.Inventory.storage, OnSelectPlayerItem, playerInventoryGrid.transform);
        infoPanel.ResetInfo();
    }
    
    private void OnSelectPlayerItem(Item _item)
    {
        infoPanel.OnInfo(new DeepStorageInfoData()
        {
            item = _item,
            onFirstAction = new DeepStorageInfoAction
            {
                name = "Transfer",
                action = (item) =>
                {
                    player.Inventory.RemoveByItem(_item);
                    activeInventory.AddByItem(_item);
                    RerenderInventory();
                }
            }
        });
    }
    
    protected override void OnSelectItem(Item _item)
    {
        infoPanel.OnInfo(new DeepStorageInfoData()
        {
            item = _item,
            onFirstAction = new DeepStorageInfoAction
            {
                name = "Transfer",
                action = (item) =>
                {
                    activeInventory.RemoveByItem(_item);
                    player.Inventory.AddByItem(_item);
                    RerenderInventory();
                }
            }
        });
    }
  
}
