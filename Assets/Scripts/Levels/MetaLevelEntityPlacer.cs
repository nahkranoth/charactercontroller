using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class MetaLevelEntityPlacer : MonoBehaviour
{
    public LevelEntityPlacer entityPlacer;

    public EntityCollection containerEntities;

    public void Generate(LevelEntityPlacer placer, MetaTilemapGenerator generator)
    {
        // var possiblePlaces = generator.background.GetAllTilesOfKey(TileLibraryKey.Floor);
        // Vector3 center;
        //
        // placer.ClearContainers();
        // for (int i = 0; i < 5; i++)
        // {
        //     var ranI = Random.Range(0, possiblePlaces.Count);
        //     var place = possiblePlaces[ranI];
        //     if (generator.collision.IsEmpty(place))
        //     {
        //         center = generator.GetWorldPosition() + place * transform.localScale.x;
        //         placer.GenerateContainer(containerEntities.collection[0], center);
        //     }
        // }
        //
    }
}
