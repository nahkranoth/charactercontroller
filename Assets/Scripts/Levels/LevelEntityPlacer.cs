using System.Collections.Generic;
using UnityEngine;

public class LevelEntityPlacer : MonoBehaviour
{
    public Dictionary<Vector3Int, GameObject> containerPool = new Dictionary<Vector3Int, GameObject>();
    
    //Naive approach; no pooling
    public void RemoveAt(Vector3Int position)
    {
        GameObject target;
        if (containerPool.TryGetValue(position, out target))
        {
            Destroy(target);
            containerPool[position] = null;
        }
    }
    
    public void GenerateContainer(GameObject obj, Vector3Int position)
    {
        var container = Instantiate(obj, transform);
        container.transform.localPosition = position;
        containerPool[position] = container;
    }
}
