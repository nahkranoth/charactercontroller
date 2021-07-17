using UnityEngine;

public class WorldController : MonoBehaviour
{
    public GameObject chestPrefab;
    
    private void Awake()
    {
        WorldGraph.Subscribe(this, typeof(WorldController));
    }

    public void SpawnChest(Vector3 position)
    {
        //Debug.Log("Spawn Chest");
        //Instantiate(chestPrefab, position, Quaternion.identity);
    }
    
}
