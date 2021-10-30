using System;
using UnityEngine;

public class Deepstorage : MonoBehaviour
{
    public GameObject deepStoragePrefab;
    public GameObject mainPanel;
    public GameObject inventoryGrid;

    public DeepstorageInfo infoPanel;
    
    private PlayerController player;
    private InputController input;
    
    private bool asShop;
    
    private void Awake()
    {
        WorldGraph.Subscribe(this, typeof(Deepstorage));
    }

    void Start()
    {
        player = WorldGraph.Retrieve(typeof(PlayerController)) as PlayerController;
        input = WorldGraph.Retrieve(typeof(InputController)) as InputController;
        input.OpenDeepStorageAsPlayer -= ToggleVisibleAsPlayer;
        input.OpenDeepStorageAsPlayer += ToggleVisibleAsPlayer;
        mainPanel.SetActive(false);
    }

    private void ToggleVisibleAsPlayer()
    {
        ToggleVisible(player.inventory, false);
        infoPanel.info.text = "Inventory";
    }

    public void SetVisible(EntityInventory inventory, bool _asShop)
    {
        if(mainPanel.activeSelf) return;
        infoPanel.info.text = _asShop ? "Welcome to my shop" : "Player inventory";
        input.BlockExcept(InputType.OpenInventory);
        mainPanel.SetActive(true);
        InstantiateItems(inventory);
        asShop = _asShop;
    }
    
    public void ToggleVisible(EntityInventory inventory, bool _asShop)
    {
        mainPanel.SetActive(!mainPanel.activeSelf);
        
        if (mainPanel.activeSelf) input.BlockExcept(InputType.OpenInventory);
        else input.LiftBlockExcept();
        
        asShop = _asShop;
        InstantiateItems(inventory);
    }

    private void InstantiateItems(EntityInventory inventory)
    {
        foreach (var itm in inventory.storage)
        {
            var ds = Instantiate(deepStoragePrefab, inventoryGrid.transform).GetComponent<DeepstorageItem>();
            ds.Apply(itm);
            ds.OnSelect -= OnSelectItem;
            ds.OnSelect += OnSelectItem;
        }

        if (!mainPanel.activeSelf)
        {
            foreach (Transform child in inventoryGrid.transform)
            {
                child.GetComponent<DeepstorageItem>().OnSelect -= OnSelectItem;
                Destroy(child.gameObject);
            }
        }
    }

    private void OnSelectItem(Item _item)
    {
        infoPanel.OnInfo(new DeepStorageInfoData()
        {
            isShop = asShop,
            item = _item,
            firstActionName = "Buy",
            onFirstAction = OnBuy
        });
    }

    private void OnBuy(Item item)
    {
        if (player.inventory.Money < item.price) return;
        player.inventory.ChangeMoney(-item.price);
        player.inventory.AddByItem(item.DeepCopy());
    }
}
