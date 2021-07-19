using System;
using UnityEngine;

public class WorldController : MonoBehaviour
{
    public GameObject chestPrefab;

    public bool npcActive = true;
    public bool playerActive = true;
    private InputController input;
    private void Awake()
    {
        WorldGraph.Subscribe(this, typeof(WorldController));
    }

    private void Start()
    {
        input = WorldGraph.Retrieve(typeof(InputController)) as InputController;
        
        input.OpenMenu -= ToggleNPCState;
        input.OpenMenu += ToggleNPCState;
        input.OpenMenu -= TogglePlayerState;
        input.OpenMenu += TogglePlayerState;
        
    }

    public void TogglePlayerState()
    {
        playerActive = !playerActive;
    }

    public void ToggleNPCState()
    {
        npcActive = !npcActive;
    }
    
    public void SpawnChest(Vector3 position)
    {
        //Debug.Log("Spawn Chest");
        //Instantiate(chestPrefab, position, Quaternion.identity);
    }
    
}
