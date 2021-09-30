using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

public class TilemapBackgroundGenerator : TilemapGenerator
{
    public TileBase grass;
    public TileBase road;
    
    private int[,] blueprint;

    private void Start()
    {
        tileBaseMap.Add(1, grass);
        tileBaseMap.Add(2, road);
        Generate();
    }

    public void Generate()
    {
        blueprint = new int[size.x,size.y];
        InitBlueprint(ref blueprint);
        AddRoad(ref blueprint);
        BuildMap(blueprint);
    }

    private static void AddRoad(ref int[,] map)
    {
        int xSize = map.GetUpperBound(0);
        int currentX = xSize / 2;
        int ySize = map.GetUpperBound(1);
        int halfY = ySize / 2;
        
        for (int y = 0; y < halfY+1; y++)
        {
            if(currentX > 0 && currentX < xSize) map[currentX, y] = 2;
            if(currentX-1 > 0 && currentX-1 < xSize) map[currentX-1, y] = 2;
            if(currentX+1 > 0 && currentX+1 < xSize) map[currentX+1, y] = 2;
            //Mirror
            if(currentX > 0 && currentX < xSize) map[currentX, ySize-1-y] = 2;
            if(currentX-1 > 0 && currentX-1 < xSize) map[currentX-1, ySize-1-y] = 2;
            if(currentX+1 > 0 && currentX+1 < xSize) map[currentX+1, ySize-1-y] = 2;
            
            currentX += Random.Range(-1, 2);
        }
    }
}
