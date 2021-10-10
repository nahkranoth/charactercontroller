using UnityEngine;

public class MetaLevelEntityPlacer : MonoBehaviour
{
    public LevelEntityPlacer entityPlacerTick;
    public LevelEntityPlacer entityPlacerTack;

    public EntityCollection containerEntities;

    public void GenerateTick(MetaTilemapGenerator generator)
    {
        Generate(entityPlacerTick, generator);
    }
    
    public void GenerateTack(MetaTilemapGenerator generator)
    {
        Generate(entityPlacerTack, generator);
    }
    
    public void Generate(LevelEntityPlacer placer, MetaTilemapGenerator generator)
    {
        var center = generator.GetWorldPosition() + new Vector3(
            generator.tilemapSize.x/2 * transform.localScale.x, 
            generator.tilemapSize.y/2 * transform.localScale.y, 
            0);
        
        placer.GenerateContainer(containerEntities.collection[0], center);
    }
}
