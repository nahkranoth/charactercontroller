using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelEntityPlacer : MonoBehaviour
{
    public Dictionary<Vector3Int, InteractionCollectable> collectablePool = new Dictionary<Vector3Int, InteractionCollectable>();
    public Dictionary<Vector3Int, InteractionCollectable> ignoreCollectablePool = new Dictionary<Vector3Int, InteractionCollectable>();
    
    public Dictionary<Vector3Int, GameObject> enemyPool = new Dictionary<Vector3Int, GameObject>();

    private MetaTilemapGenerator tilemapGenerator;
    
    private void Start()
    {
        tilemapGenerator = WorldGraph.Retrieve(typeof(MetaTilemapGenerator)) as MetaTilemapGenerator;
    }

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
    
    public void RemoveEnemyAt(Vector3Int position)
    {
        GameObject target;
        if (enemyPool.TryGetValue(position, out target))
        {
            Destroy(target);
            enemyPool.Remove(position);
        }
    }

    private void CollectCollectable(InteractionCollectable collectable)
    {
        if (collectablePool.ContainsValue(collectable))
        {
            var result = collectablePool.First(x => x.Value == collectable);
            RemoveCollectableAt(result.Key);
            ignoreCollectablePool.Add(result.Key, result.Value);
        }
    }

    public void GenerateCollectable(GameObject obj, Vector3 position)
    {
        GenerateCollectable(obj, Vector3Int.RoundToInt(position));
    }

    public void GenerateCollectable(GameObject obj, Vector3Int position)
    {
        if (collectablePool.ContainsKey(position)) return;//is already active
        if (ignoreCollectablePool.ContainsKey(position)) return;//is already collected
        var container = Instantiate(obj, transform);
        var collectable = container.GetComponent(typeof(InteractionCollectable)) as InteractionCollectable;
        container.transform.localPosition = position;
        collectablePool[position] = collectable;
        collectable.OnRemove -= CollectCollectable;
        collectable.OnRemove += CollectCollectable;
    }
    
    public void GenerateNPC(GameObject obj, Vector3Int position)
    {
        if (enemyPool.ContainsKey(position)) return;//is already active
        var container = Instantiate(obj, transform);
        container.transform.localPosition = position;
        enemyPool[position] = container;
    }
}
