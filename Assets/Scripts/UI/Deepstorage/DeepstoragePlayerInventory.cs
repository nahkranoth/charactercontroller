using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DeepstoragePlayerInventory : AbstractDeepStorageScreen
{
    
    public Button saveButton;
    public Button loadButton;
    public TextMeshProUGUI storageCapText;
    
    private PlayerController player;
    private SaveLoad saveLoad;
    private void Awake()
    {
        WorldGraph.Subscribe(this, typeof(DeepstoragePlayerInventory));
    }
    
    protected override void AfterStart()
    {
        input.OpenDeepStorageAsPlayer -= ToggleShow;
        input.OpenDeepStorageAsPlayer += ToggleShow;
        
        player = WorldGraph.Retrieve(typeof(PlayerController)) as PlayerController;
        
        saveButton.onClick.AddListener(SaveClicked);
        loadButton.onClick.AddListener(LoadClicked);
        
        saveLoad = WorldGraph.Retrieve(typeof(SaveLoad)) as SaveLoad;
    }

    private void SaveClicked()
    {
        saveLoad.Save();
    }
    
    private void LoadClicked()
    {
        var playerStatus = saveLoad.Load();
        player.stateController.OverrideStatus(playerStatus); 
        activeInventory = player.Inventory;
        RerenderInventory();
    }

    private void ToggleShow()
    {
        if (mainPanel.activeSelf)
        {
            Hide();
            return;
        }
        Show();
    }

    public void Show()
    {
        if(mainPanel.activeSelf) return;
        activeInventory = player.Inventory;
        secondInventory = player.Wearing;
        infoPanel.info.text = "Player inventory";
        SetStorageCap();
        mainPanel.SetActive(true);
        Debug.Log("Open UI");
        input.ChangeScheme("UI");
        input.OnCloseUI += Hide;
        InstantiateItems(activeInventory.storage, OnSelectItem, inventoryGrid.transform);
        InstantiateItems(secondInventory.storage, OnSecondarySelectItem, secondInventoryGrid.transform);
    }
    
    void RerenderInventory()
    {
        DestroyItems(OnSelectItem, inventoryGrid.transform);
        DestroyItems(OnSecondarySelectItem, secondInventoryGrid.transform);
        InstantiateItems(activeInventory.storage, OnSelectItem, inventoryGrid.transform);
        InstantiateItems(secondInventory.storage, OnSecondarySelectItem, secondInventoryGrid.transform);
        SetStorageCap();
        infoPanel.ResetInfo();
    }

    private void SetStorageCap()
    {
        storageCapText.text = $"{activeInventory.TotalItemWeight()}/{player.stateController.MaxCarryWeight}";
    }

    private void OnSecondarySelectItem(Item _item)
    {
        if (_item.wearable)
        {
            infoPanel.OnInfo(new DeepStorageInfoData()
            {
                item = _item,
                onFirstAction = new DeepStorageInfoAction
                {
                    name = "Unwear",
                    action = (item) =>
                    {
                        player.Inventory.AddByItem(_item);
                        player.Wearing.RemoveByItem(_item);
                        RerenderInventory();
                        infoPanel.ResetInfo();
                        player.stateController.StatusUpdate();
                    }
                }
            });
        }
    }
    
    protected override void OnSelectItem(Item _item)
    {
        DeepStorageInfoAction firstAction = null;
        
        firstAction = new DeepStorageInfoAction
        {
            name = "Destroy",
            action = (item) =>
            {
                player.Inventory.RemoveByItem(_item);
                RerenderInventory();
            }
        };
        
        if (_item.wearable)
        {
            firstAction = new DeepStorageInfoAction
            {
                name = "Wear",
                action = (item) =>
                {
                    player.Inventory.RemoveByItem(_item);
                    player.Wearing.AddByItem(_item);
                    RerenderInventory();
                    player.stateController.StatusUpdate();
                }
            };
        }

        infoPanel.OnInfo(new DeepStorageInfoData()
        {
            item = _item,
            onFirstAction = firstAction
        });
    }
  
}
