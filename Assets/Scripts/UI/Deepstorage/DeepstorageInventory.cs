using Codice.CM.WorkspaceServer.DataStore.Merge;
using UnityEngine;
using UnityEngine.UI;

public class DeepstorageInventory : AbstractDeepStorageScreen
{
    
    public Button saveButton;
    public Button loadButton;

    private PlayerController player;
    private SaveLoad saveLoad;
    private void Awake()
    {
        WorldGraph.Subscribe(this, typeof(DeepstorageInventory));
    }
    
    protected override void AfterStart()
    {
        input.OpenDeepStorageAsPlayer -= ToggleShow;
        input.OpenDeepStorageAsPlayer += ToggleShow;
        
        player = WorldGraph.Retrieve(typeof(PlayerController)) as PlayerController;
        
        saveButton.onClick.AddListener(SaveClicked);
        loadButton.onClick.AddListener(LoadClicked);
        
        saveLoad = WorldGraph.Retrieve(typeof(SaveLoad)) as SaveLoad;
        Debug.Log(saveLoad);
    }

    private void SaveClicked()
    {
        saveLoad.Save();
    }
    
    private void LoadClicked()
    {
        var playerStatus = saveLoad.Load();
        player.status = Merger.CloneAndMerge(player.status, playerStatus);
        player.status.Update();
        activeInventory = player.status.inventory;
        RerenderInventory();
    }

    private void ToggleShow()
    {
        if (mainPanel.activeSelf)
        {
            Hide();
            return;
        }
        Show(player.status.inventory);
    }

    public void Show(EntityInventory inventory)
    {
        if(mainPanel.activeSelf) return;
        activeInventory = inventory;
        infoPanel.info.text = "Player inventory";
        input.BlockExcept(InputType.OpenInventory);
        mainPanel.SetActive(true);
        InstantiateItems(activeInventory, OnSelectItem);
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
                    player.status.inventory.RemoveByItem(_item);
                    RerenderInventory();
                }
            }
        });
    }
  
}
