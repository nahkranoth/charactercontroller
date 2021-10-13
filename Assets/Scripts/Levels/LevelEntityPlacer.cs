using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelEntityPlacer : MonoBehaviour
{
    public Dictionary<Vector3Int, InteractionCollectable> collectablePool = new Dictionary<Vector3Int, InteractionCollectable>();
    public Dictionary<Vector3Int, InteractionCollectable> ignorePool = new Dictionary<Vector3Int, InteractionCollectable>();
    
    //Naive approach; no pooling
    public void RemoveCollectableAt(Vector3Int position)
    {
        InteractionCollectable target;
        if (collectablePool.TryGetValue(position, out target))
        {
            target.OnRemove -= CollectCollectable;
            Destroy(target.gameObject);
            collectablePool.Remove(position);
        }
    }

    private void CollectCollectable(InteractionCollectable collectable)
    {
        if (collectablePool.ContainsValue(collectable))
        {
            var result = collectablePool.First(x => x.Value == collectable);
            RemoveCollectableAt(result.Key);
            ignorePool.Add(result.Key, result.Value);
        }
    }
    
    public void GenerateCollectable(GameObject obj, Vector3Int position)
    {
        if (collectablePool.ContainsKey(position)) return;//is already active
        if (ignorePool.ContainsKey(position)) return;//is already collected
        var container = Instantiate(obj, transform);
        var collectable = container.GetComponent(typeof(InteractionCollectable)) as InteractionCollectable;
        container.transform.localPosition = position;
        collectablePool[position] = collectable;
        collectable.OnRemove -= CollectCollectable;
        collectable.OnRemove += CollectCollectable;
    }
}
