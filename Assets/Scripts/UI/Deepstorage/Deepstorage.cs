using System;
using UnityEngine;

public class Deepstorage : MonoBehaviour
{
    public GameObject deepStoragePrefab;
    public GameObject mainPanel;
    public GameObject inventoryGrid;

    public DeepstorageInfo infoPanel;
    
    private PlayerController player;

    private bool asShop;
    
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

    public void SetVisible(EntityInventory inventory, bool _asShop)
    {
        if(mainPanel.activeSelf) return;
        infoPanel.info.text = _asShop ? "Welcome to my shop" : "Player inventory";
        mainPanel.SetActive(true);
        InstantiateItems(inventory);
        asShop = _asShop;
    }
    
    public void ToggleVisible(EntityInventory inventory, bool _asShop)
    {
        mainPanel.SetActive(!mainPanel.activeSelf);
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
