using System;
using UnityEngine;

public class Deepstorage : MonoBehaviour
{
    public GameObject deepStoragePrefab;
    public GameObject mainPanel;
    public GameObject playerInventory;
    
    private PlayerController player;

    private void Awake()
    {
        WorldGraph.Subscribe(this, typeof(Deepstorage));
    }

    void Start()
    {
        player = WorldGraph.Retrieve(typeof(PlayerController)) as PlayerController;
        var inputController = WorldGraph.Retrieve(typeof(InputController)) as InputController;
        inputController.OpenDeepStorage -= ToggleVisible;
        inputController.OpenDeepStorage += ToggleVisible;
        mainPanel.SetActive(false);
    }
    
    private void ToggleVisible()
    {
        mainPanel.SetActive(!mainPanel.activeSelf);
        foreach (var itm in player.inventory.storage)
        {
            var ds = Instantiate(deepStoragePrefab, playerInventory.transform).GetComponent<DeepstorageItem>();
            ds.Apply(itm);
        }

        if (!mainPanel.activeSelf)
        {
            foreach (Transform child in playerInventory.transform) {
                Destroy(child.gameObject);
            }
        }
        
    }
}
