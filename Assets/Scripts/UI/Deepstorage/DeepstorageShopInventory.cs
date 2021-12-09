using UnityEngine.UI;

public class DeepstorageShopInventory : AbstractDeepStorageScreen
{
    
    public Button buyButton;
    public Button sellButton;

    private bool asShop;
    private bool asShopSellState;

    private void Awake()
    {
        WorldGraph.Subscribe(this, typeof(DeepstorageShopInventory));
    }
    
    protected override void AfterStart()
    {
    }
    
    public void Show(EntityInventory inventory)
    {
        if(mainPanel.activeSelf) return;
        activeInventory = inventory;
        infoPanel.info.text = "Welcome to my shop";
        mainPanel.SetActive(true);
        InstantiateItems(activeInventory.storage,OnSelectItem, inventoryGrid.transform);
        buyButton.gameObject.SetActive(true);
        sellButton.gameObject.SetActive(true);
        asShop = true;
        input.ChangeScheme("UI");
        input.OnCloseUI += Hide;
        sellButton.onClick.AddListener(SetShopToSellState);
        buyButton.onClick.AddListener(SetShopToBuyState);
        asShopSellState = false;
    }

    private void SetShopToSellState()
    {
        asShopSellState = true;
        DestroyItems(OnSelectItem, inventoryGrid.transform);
        InstantiateItems(player.Inventory.storage, OnSelectItem, inventoryGrid.transform);
    }

    private void SetShopToBuyState()
    {
        asShopSellState = false;
        DestroyItems(OnSelectItem, inventoryGrid.transform);
        InstantiateItems(activeInventory.storage, OnSelectItem, inventoryGrid.transform);
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
        if (player.stateController.Money < item.price) return;
        if (!player.stateController.HasCarrySpace(item.weight))
        {
            infoPanel.info.text = "No space in inventory";
            return;
        }
        player.stateController.ChangeMoney(-item.price);
        player.Inventory.AddByItem(item.DeepCopy());
    }
    
    private void OnSell(Item item)
    {
        if (!player.Inventory.Exists(item)) return;
        player.stateController.ChangeMoney(item.price);
        player.Inventory.RemoveByItem(item);
        RerenderInventory();
    }
}
