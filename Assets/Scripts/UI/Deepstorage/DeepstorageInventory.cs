using UnityEngine;
using UnityEngine.UI;

public class DeepstorageInventory : MonoBehaviour
{
    public GameObject deepStoragePrefab;
    public GameObject mainPanel;
    public GameObject inventoryGrid;
    public DeepstorageInfo infoPanel;
    
    private PlayerController player;
    private InputController input;
    private EntityInventory activeShopInventory;
    
    private void Awake()
    {
        WorldGraph.Subscribe(this, typeof(DeepstorageInventory));
    }

    void Start()
    {
        player = WorldGraph.Retrieve(typeof(PlayerController)) as PlayerController;
        input = WorldGraph.Retrieve(typeof(InputController)) as InputController;
        input.OpenDeepStorageAsPlayer -= ToggleShow;
        input.OpenDeepStorageAsPlayer += ToggleShow;
        mainPanel.SetActive(false);
    }

    private void ToggleShow()
    {
        if (mainPanel.activeSelf)
        {
            Hide();
            return;
        }

        Show(player.inventory);

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
        infoPanel.info.text = "Player inventory";
        input.BlockExcept(InputType.OpenInventory);
        mainPanel.SetActive(true);
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
        var activeInventory = player.inventory;
        InstantiateItems(activeInventory);
    }

    private void OnSelectItem(Item _item)
    {
        
    }
}
