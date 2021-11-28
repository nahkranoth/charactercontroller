using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractDeepStorageScreen:MonoBehaviour
{
    public GameObject mainPanel;
    public DeepstorageInfo infoPanel;
    public GameObject deepStoragePrefab;
    public GameObject inventoryGrid;
    public GameObject secondInventoryGrid;

    protected PlayerController player;
    protected InputController input;
    
    protected EntityInventory activeInventory;
    protected EntityInventory secondInventory;
    
    public void Start()
    {
        player = WorldGraph.Retrieve(typeof(PlayerController)) as PlayerController;
        input = WorldGraph.Retrieve(typeof(InputController)) as InputController;
        mainPanel.SetActive(false);
        AfterStart();
    }
    
    protected abstract void AfterStart();
    
    protected void Hide()
    {
        input.LiftBlockExcept();
        DestroyItems(OnSelectItem, inventoryGrid.transform);
        if(secondInventoryGrid) DestroyItems(OnSelectItem, secondInventoryGrid.transform);
        mainPanel.SetActive(false);
    }
    
    protected void InstantiateItems(List<Item> storage, Action<Item> OnSelectItem, Transform target)
    {
        foreach (var itm in storage)
        {
            var ds = Instantiate(deepStoragePrefab, target).GetComponent<DeepstorageItem>();
            ds.Apply(itm);
            ds.OnSelect -= OnSelectItem;
            ds.OnSelect += OnSelectItem;
        }

        if (!mainPanel.activeSelf)
        {
            DestroyItems(OnSelectItem, inventoryGrid.transform);
        }
    }
    
    protected void DestroyItems(Action<Item> OnSelectItem, Transform target)
    {
        foreach (Transform child in target)
        {
            child.GetComponent<DeepstorageItem>().OnSelect -= OnSelectItem;
            Destroy(child.gameObject);
        }
    }
      
    protected void RerenderInventory()
    {
        DestroyItems(OnSelectItem, inventoryGrid.transform);
        InstantiateItems(activeInventory.storage, OnSelectItem, inventoryGrid.transform);
    }

    protected abstract void OnSelectItem(Item _item);


}
