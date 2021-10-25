using System;
using UnityEngine;

public class Deepstorage : MonoBehaviour
{
    public GameObject deepStoragePrefab;
    public GameObject mainPanel;
    public GameObject inventoryGrid;
    
    private PlayerController player;

    private void Awake()
    {
        WorldGraph.Subscribe(this, typeof(Deepstorage));
    }

    void Start()
    {
        player = WorldGraph.Retrieve(typeof(PlayerController)) as PlayerController;
        var inputController = WorldGraph.Retrieve(typeof(InputController)) as InputController;
        inputController.OpenDeepStorageAsPlayer -= ToggleVisibleAsPlayer;
        inputController.OpenDeepStorageAsPlayer += ToggleVisibleAsPlayer;
        mainPanel.SetActive(false);
    }

    private void ToggleVisibleAsPlayer()
    {
        ToggleVisible(player.inventory, false);
    }

    public void SetVisible(EntityInventory inventory, bool asShop)
    {
        if(mainPanel.activeSelf) return;
        mainPanel.SetActive(true);
        InstantiateItems(inventory, asShop);
    }
    
    public void ToggleVisible(EntityInventory inventory, bool asShop)
    {
        mainPanel.SetActive(!mainPanel.activeSelf);
        InstantiateItems(inventory, asShop);
    }

    private void InstantiateItems(EntityInventory inventory, bool asShop)
    {
        foreach (var itm in inventory.storage)
        {
            var ds = Instantiate(deepStoragePrefab, inventoryGrid.transform).GetComponent<DeepstorageItem>();
            ds.Apply(itm);
        }

        if (!mainPanel.activeSelf)
        {
            foreach (Transform child in inventoryGrid.transform) {
                Destroy(child.gameObject);
            }
        }
    }
}
