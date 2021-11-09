public class DeepstorageInventory : AbstractDeepStorageScreen
{
    private void Awake()
    {
        WorldGraph.Subscribe(this, typeof(DeepstorageInventory));
    }
    
    protected override void AfterStart()
    {
        input.OpenDeepStorageAsPlayer -= ToggleShow;
        input.OpenDeepStorageAsPlayer += ToggleShow;
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
                    player.inventory.RemoveByItem(_item);
                    RerenderInventory();
                }
            }
        });
    }
  
}
