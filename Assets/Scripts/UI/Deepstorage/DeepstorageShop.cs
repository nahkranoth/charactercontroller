using UnityEngine;
using UnityEngine.UI;

public class DeepstorageShop : AbstractDeepStorageScreen
{
    
    public Button buyButton;
    public Button sellButton;

    private bool asShop;
    private bool asShopSellState;

    private void Awake()
    {
        WorldGraph.Subscribe(this, typeof(DeepstorageShop));
    }
    
    protected override void AfterStart()
    {
        input.OpenDeepStorageAsPlayer -= Hide;
        input.OpenDeepStorageAsPlayer += Hide;
    }
    
    public void Show(EntityInventory inventory)
    {
        if(mainPanel.activeSelf) return;
        activeInventory = inventory;
        infoPanel.info.text = "Welcome to my shop";
        input.BlockExcept(InputType.OpenInventory);
        mainPanel.SetActive(true);
        InstantiateItems(activeInventory,OnSelectItem, inventoryGrid.transform);
        buyButton.gameObject.SetActive(true);
        sellButton.gameObject.SetActive(true);
        asShop = true;
        sellButton.onClick.AddListener(SetShopToSellState);
        buyButton.onClick.AddListener(SetShopToBuyState);
        asShopSellState = false;
    }

    private void SetShopToSellState()
    {
        asShopSellState = true;
        DestroyItems(OnSelectItem, inventoryGrid.transform);
        InstantiateItems(player.Inventory, OnSelectItem, inventoryGrid.transform);
    }

    private void SetShopToBuyState()
    {
        asShopSellState = false;
        DestroyItems(OnSelectItem, inventoryGrid.transform);
        InstantiateItems(activeInventory, OnSelectItem, inventoryGrid.transform);
    }

    protected override void OnSelectItem(Item _item)
    {
        infoPanel.OnInfo(new DeepStorageInfoData()
        {
            item = _item,
            onFirstAction = new DeepStorageInfoAction
            {
                name =  asShopSellState ? "Sell":"Buy",
                action = (itm) =>
                {
                    if (asShopSellState) OnSell(itm);
                    else OnBuy(itm);
                }
            }
        });
    }
    
    private void OnBuy(Item item)
    {
        if (player.statusController.Money < item.price) return;
        player.statusController.ChangeMoney(-item.price);
        player.Inventory.AddByItem(item.DeepCopy());
    }
    
    private void OnSell(Item item)
    {
        if (!player.Inventory.Exists(item)) return;
        player.statusController.ChangeMoney(item.price);
        player.Inventory.RemoveByItem(item);
        RerenderInventory();
    }
}
