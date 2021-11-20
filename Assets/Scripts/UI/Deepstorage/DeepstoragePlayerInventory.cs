using Codice.CM.WorkspaceServer.DataStore.Merge;
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
        player.statusController.OverrideStatus(playerStatus); 
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
        Show(player.Inventory);
    }

    public void Show(EntityInventory inventory)
    {
        if(mainPanel.activeSelf) return;
        activeInventory = inventory;
        infoPanel.info.text = "Player inventory";
        input.BlockExcept(InputType.OpenInventory);
        SetStorageCap();
        mainPanel.SetActive(true);
        InstantiateItems(activeInventory, OnSelectItem, inventoryGrid.transform);
    }
    
    void RerenderInventory()
    {
        DestroyItems(OnSelectItem, inventoryGrid.transform);
        InstantiateItems(activeInventory, OnSelectItem, inventoryGrid.transform);
        SetStorageCap();
    }

    private void SetStorageCap()
    {
        storageCapText.text = $"{activeInventory.TotalItemWeight()}";
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
