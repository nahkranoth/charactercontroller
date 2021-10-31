using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class MetaLevelEntityPlacer : MonoBehaviour
{
    public LevelEntityPlacer entityPlacer;

    private List<Vector3Int> possiblePlaces;

    private GeneratorSet generatorSet;

    private void Awake()
    {
        WorldGraph.Subscribe(this, typeof(MetaLevelEntityPlacer));
    }

    public void RemoveAt(Vector3Int position)
    {
        entityPlacer.RemoveCollectableAt(position);
        entityPlacer.RemoveEnemyAt(position);
    }
    
    public void Generate(MetaTilemapGenerator generator, Vector3Int root, GeneratorSet set)
    {
        generatorSet = set;
        possiblePlaces = generator.background.GetAllTilesOfKey(TileLibraryKey.Floor);
        GenerateContainers(generator, root);
        GenerateEnemies(generator, root);
    }

    private void GenerateEnemies(MetaTilemapGenerator generator, Vector3Int root)
    {
        Vector3Int spawnPos;
        
        for (int i = 0; i < generatorSet.npcDensity; i++)
        {
            var ranI = Random.Range(0, possiblePlaces.Count);
            var place = possiblePlaces[ranI];
            if (generator.collision.IsEmpty(place))
            {
                var vecToInt = generator.background.tilemap.CellToLocal(place);
                spawnPos = new Vector3Int((int)vecToInt.x, (int)vecToInt.y, (int)vecToInt.z) + root;
                entityPlacer.GenerateNPC(generatorSet.npcs.GetRandom(), spawnPos);
            }
        }
    }

    private void GenerateContainers(MetaTilemapGenerator generator, Vector3Int root)
    {
        Vector3Int spawnPos;

        for (int i = 0; i < generatorSet.containerDensity; i++)
        {
            var ranI = Random.Range(0, possiblePlaces.Count);
            var place = possiblePlaces[ranI];
            if (generator.collision.IsEmpty(place))
            {
                var vecToInt = generator.background.tilemap.CellToLocal(place);
                spawnPos = new Vector3Int((int)vecToInt.x, (int)vecToInt.y, (int)vecToInt.z) + root;
                entityPlacer.GenerateCollectable(generatorSet.containers.GetRandom(), spawnPos);
            }
        }
    }
}
