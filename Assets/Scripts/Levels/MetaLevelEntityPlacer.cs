using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class MetaLevelEntityPlacer : MonoBehaviour
{
    public LevelEntityPlacer entityPlacer;

    public EntityCollection containerEntities;
    public EntityCollection enemyEntities;

    private List<Vector3Int> possiblePlaces;

    public void RemoveAt(Vector3Int position)
    {
        entityPlacer.RemoveCollectableAt(position);
        entityPlacer.RemoveEnemyAt(position);
    }
    
    public void Generate(MetaTilemapGenerator generator, Vector3Int root)
    {
        possiblePlaces = generator.background.GetAllTilesOfKey(TileLibraryKey.Floor);
        GenerateContainers(generator, root);
        GenerateEnemies(generator, root);
    }

    private void GenerateEnemies(MetaTilemapGenerator generator, Vector3Int root)
    {
        Vector3Int spawnPos;
        
        for (int i = 0; i < 5; i++)
        {
            var ranI = Random.Range(0, possiblePlaces.Count);
            var place = possiblePlaces[ranI];
            if (generator.collision.IsEmpty(place))
            {
                var vecToInt = generator.background.tilemap.CellToLocal(place);
                spawnPos = new Vector3Int((int)vecToInt.x, (int)vecToInt.y, (int)vecToInt.z) + root;
                entityPlacer.GenerateEnemy(enemyEntities.GetRandom(), spawnPos);
            }
        }
    }

    private void GenerateContainers(MetaTilemapGenerator generator, Vector3Int root)
    {
        Vector3Int spawnPos;

        for (int i = 0; i < 5; i++)
        {
            var ranI = Random.Range(0, possiblePlaces.Count);
            var place = possiblePlaces[ranI];
            if (generator.collision.IsEmpty(place))
            {
                var vecToInt = generator.background.tilemap.CellToLocal(place);
                spawnPos = new Vector3Int((int)vecToInt.x, (int)vecToInt.y, (int)vecToInt.z) + root;
                entityPlacer.GenerateCollectable(containerEntities.GetRandom(), spawnPos);
            }
        }
    }
}
