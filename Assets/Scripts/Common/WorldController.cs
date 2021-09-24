using System;
using UnityEngine;

public class WorldController : MonoBehaviour
{
    public GameObject chestPrefab;
    public GameObject menuWheelController;
    public bool menuActive = false;
    public bool npcActive = true;
    public bool playerActive = true;
    private InputController input;
    
    public Action<bool> OnToggleMenu;
    
    private void Awake()
    {
        WorldGraph.Subscribe(this, typeof(WorldController));
    }

    private void Start()
    {
        input = WorldGraph.Retrieve(typeof(InputController)) as InputController;
        
        input.OpenMenu -= ToggleMenu;
        input.OpenMenu += ToggleMenu;
    }

    public void ToggleMenu()
    {
        menuActive = !menuActive;
        playerActive = !menuActive;
        npcActive = !menuActive;
        OnToggleMenu?.Invoke(menuActive);
    }
    
    public void SpawnChest(Vector3 position)
    {
        //Debug.Log("Spawn Chest");
        Instantiate(chestPrefab, position, Quaternion.identity);
    }
    
}
