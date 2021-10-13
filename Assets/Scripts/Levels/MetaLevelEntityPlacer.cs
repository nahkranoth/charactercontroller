using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class MetaLevelEntityPlacer : MonoBehaviour
{
    public LevelEntityPlacer entityPlacer;

    public EntityCollection containerEntities;

    public void RemoveAt(Vector3Int position)
    {
        entityPlacer.RemoveCollectableAt(position);
    }
    
    public void Generate(MetaTilemapGenerator generator, Vector3Int root)
    {
        var possiblePlaces = generator.background.GetAllTilesOfKey(TileLibraryKey.Floor);

        Vector3Int spawnPos;

        for (int i = 0; i < 5; i++)
        {
            var ranI = Random.Range(0, possiblePlaces.Count);
            var place = possiblePlaces[ranI];
            if (generator.collision.IsEmpty(place))
            {
                var vecToInt = generator.background.tilemap.CellToLocal(place);
                spawnPos = new Vector3Int((int)vecToInt.x, (int)vecToInt.y, (int)vecToInt.z) + root;
                entityPlacer.GenerateCollectable(containerEntities.collection[0], spawnPos);
            }
        }
    }
}
