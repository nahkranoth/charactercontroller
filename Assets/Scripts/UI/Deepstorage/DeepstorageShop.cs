using UnityEngine;
using UnityEngine.UI;

public class DeepstorageShop : MonoBehaviour
{
    public GameObject deepStoragePrefab;
    public GameObject mainPanel;
    public GameObject inventoryGrid;
    public DeepstorageInfo infoPanel;
    public Button buyButton;
    public Button sellButton;
    
    private PlayerController player;
    private InputController input;
    private EntityInventory activeShopInventory;
    
    private bool asShop;
    private bool asShopSellState;
    
    private void Awake()
    {
        WorldGraph.Subscribe(this, typeof(DeepstorageShop));
    }

    void Start()
    {
        player = WorldGraph.Retrieve(typeof(PlayerController)) as PlayerController;
        input = WorldGraph.Retrieve(typeof(InputController)) as InputController;
        input.OpenDeepStorageAsPlayer -= Hide;
        input.OpenDeepStorageAsPlayer += Hide;
        mainPanel.SetActive(false);
    }

    private void Hide()
    {
        input.LiftBlockExcept();
        DestroyItems();
        mainPanel.SetActive(false);
    }

    public void Show(EntityInventory inventory)
    {
        if(mainPanel.activeSelf) return;
        activeShopInventory = inventory;
        infoPanel.info.text = "Welcome to my shop";
        input.BlockExcept(InputType.OpenInventory);
        mainPanel.SetActive(true);
        InstantiateItems(activeShopInventory);
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
        DestroyItems();
        InstantiateItems(player.inventory);
    }
    
    private void SetShopToBuyState()
    {
        asShopSellState = false;
        DestroyItems();
        InstantiateItems(activeShopInventory);
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
            DestroyItems();
        }
    }

    private void DestroyItems()
    {
        foreach (Transform child in inventoryGrid.transform)
        {
            child.GetComponent<DeepstorageItem>().OnSelect -= OnSelectItem;
            Destroy(child.gameObject);
        }
    }

    private void RerenderInventory()
    {
        DestroyItems();
        var activeInventory = asShopSellState ? player.inventory : activeShopInventory;
        InstantiateItems(activeInventory);
    }

    private void OnSelectItem(Item _item)
    {
        infoPanel.OnInfo(new DeepStorageInfoData()
        {
            isShop = asShop,
            item = _item,
            firstActionName = asShopSellState ? "Sell":"Buy",
            onFirstAction = (itm) =>
            {
                if (asShopSellState) OnSell(itm);
                else OnBuy(itm);
            }
        });
    }
    
    private void OnBuy(Item item)
    {
        if (player.inventory.Money < item.price) return;
        player.inventory.ChangeMoney(-item.price);
        player.inventory.AddByItem(item.DeepCopy());
    }
    
    private void OnSell(Item item)
    {
        if (!player.inventory.Exists(item)) return;
        player.inventory.ChangeMoney(item.price);
        player.inventory.RemoveByItem(item);
        RerenderInventory();
    }
}
