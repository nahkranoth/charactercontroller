using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using UnityEngine;

public class LevelEntityPlacer : MonoBehaviour
{
    public List<GameObject> containerPool;
    
    //Naive approach; no pooling
    public void ClearContainers()
    {
        foreach (var container in containerPool) Destroy(container);
        containerPool = new List<GameObject>();
    }
    
    public void GenerateContainer(GameObject obj, Vector3 position)
    {
        var container = Instantiate(obj, transform);
        container.transform.position = position;
        containerPool.Add(container);
    }
}
