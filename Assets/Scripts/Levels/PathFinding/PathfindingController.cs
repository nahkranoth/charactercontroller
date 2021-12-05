using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

public static class ADJACENTCELLS
{
    public static Vector3Int UP = new Vector3Int(0, 1, 0);
    public static Vector3Int UPRIGHT = new Vector3Int(1, 1, 0);
    public static Vector3Int RIGHT = new Vector3Int(1, 0, 0);
    public static Vector3Int DOWNRIGHT = new Vector3Int(1, -1, 0);
    public static Vector3Int DOWN = new Vector3Int(0, -1, 0);
    public static Vector3Int DOWNLEFT = new Vector3Int(-1, -1, 0);
    public static Vector3Int LEFT = new Vector3Int(-1, 0, 0);
    public static Vector3Int UPLEFT = new Vector3Int(-1, 1, 0);
    public static List<Vector3Int> ALL = new List<Vector3Int>() {UP,UPRIGHT,RIGHT,DOWNRIGHT,DOWN,DOWNLEFT,LEFT,UPLEFT};
}

public class PathfindingController : MonoBehaviour
{
    private PlayerController player;
    private Dictionary<Vector3Int, CellData> cellMap;
    public BackgroundTilemapGenerator bgGenerator;
    public CollisionTilemapGenerator collGenerator;
    private LevelRepeater repeater;
    List<CellData> openList = new List<CellData>();
    List<CellData> closeList = new List<CellData>();
    public bool DEBUG = true;
    public KeyValuePair<Vector3Int, CellData>[] cellmapList;

    private void Awake()
    {
        WorldGraph.Subscribe(this, typeof(PathfindingController));
    }

    private void Start()
    {
        //TODO sometimes OnGenerate is already called
        repeater = WorldGraph.Retrieve(typeof(LevelRepeater)) as LevelRepeater;
        repeater.OnGenerate -= Generate;
        repeater.OnGenerate += Generate;
    }

    public void Generate()
    {
        player = WorldGraph.Retrieve(typeof(PlayerController)) as PlayerController;
        cellMap = GetTilemapAsCellmap(
            bgGenerator.tilemap,
            collGenerator.tilemap
        );
        cellmapList = cellMap.ToArray();
    }

    private Dictionary<Vector3Int, CellData> GetTilemapAsCellmap(Tilemap tilemap, Tilemap collision)
    {
        var cellMap = new Dictionary<Vector3Int, CellData>();

        for (var x = tilemap.cellBounds.min.x; x < tilemap.cellBounds.max.x; x++)
        {
            for (var y = tilemap.cellBounds.min.y; y < tilemap.cellBounds.max.y; y++)
            {
                var pos = new Vector3Int(x, y, 0);
                var tile = tilemap.GetTile(pos);
                var collTile = collision.GetTile(pos);
                if (tile)
                {
                    cellMap[pos] = new CellData
                    {
                        worldPos = tilemap.GetCellCenterWorld(pos),
                        walkable = collTile == null, 
                        position = pos,
                    };
                }
            } 
        }

        return cellMap;
    }
    public List<CellData> FindPathToRandomPosByWorldPos(Vector3 worldPos, int depth=-1)
    {
        var startCell = GetFromCellMapByWorldPos(worldPos);
        var finishCell = GetRandomCell(depth, startCell.position);
        
        var result = new List<CellData>();
        if (startCell != null && finishCell != null)
        {
            result = FindPath(startCell, finishCell);
        }
        
        if(result.Count == 0) Debug.LogWarning($"Could not find Path: Start {startCell} Finish {finishCell}");
        return result;
    }
    
    public List<CellData> FindPathToTargetByWorldPos(Vector3 worldPos, Vector3 targetPos)
    {
        var startCell = GetFromCellMapByWorldPos(worldPos);
        var finishCell = GetFromCellMapByWorldPos(targetPos);
        return FindPath(startCell, finishCell);
    }
    public List<CellData> FindPath(CellData startCell, CellData finishCell)
    {
        if (startCell == null || finishCell == null)return new List<CellData>();

        openList.Clear();
        closeList.Clear();
        ClearPath();
        openList.Add(startCell);

        startCell.heuristics = (finishCell.position - startCell.position).magnitude;
        while (openList.Count > 0)
        {
            var bestCell = GetBestCell();
            openList.Remove(bestCell);
            var neighbours = GetCellNeighbours(bestCell);
            foreach (var neighbour in neighbours)
            {
                if (neighbour == null || neighbour.walkable == false)
                    continue;
                if (neighbour == finishCell)
                {
                    neighbour.parent = bestCell;
                    return ConstructPath(neighbour);
                }
                
                var neighbour_neighbours = GetCellNeighbours(bestCell);
                
                float extra_cost = 0;
                
                if (neighbour_neighbours.Any(x => x.walkable == false)) extra_cost = 1f;
                
                var g = bestCell.cost + extra_cost + (neighbour.position - bestCell.position).magnitude;
                var h = (finishCell.position - neighbour.position).magnitude;
                
                if (openList.Contains(neighbour) && neighbour.F < (g + h))
                    continue;
                if (closeList.Contains(neighbour) && neighbour.F < (g + h))
                    continue;

                neighbour.cost = g;
                neighbour.heuristics = h;
                neighbour.parent = bestCell;
                
                if (!openList.Contains(neighbour))
                    openList.Add(neighbour);
            }
            
            if (!closeList.Contains(bestCell))
                closeList.Add(bestCell);
          
        }
        return null;
    }
    
    private CellData GetBestCell ()
    {
        CellData result = null;
        float currentF = float.PositiveInfinity;

        for (int i = 0; i < openList.Count; i++)
        {
            CellData cell = openList[i];

            if (cell.F < currentF)
            {
                currentF = cell.F;
                result = cell;
            }
        }

        return result;
    }

    private void ClearPath()
    {
        foreach (var cell in cellMap)
        {
            cell.Value.parent = null;
            cell.Value.cost = 0;
            cell.Value.heuristics = 0;
        }
    }
    private List<CellData> ConstructPath(CellData destination)
    {
        var path = new List<CellData>{destination};
        var current = destination;
        var limit = 100;
        var cntr = 0;
        while (current.parent != null && cntr <= limit )
        {
            cntr++;
            if (cntr == limit)
            {
                Debug.Log("Path Constructor exceeds limit");
            }
            current = current.parent;
            path.Add(current);
            if(DEBUG) bgGenerator.ColorTileAtCell(current.position, bgGenerator.tilemap, Color.red);
        }

        path.Reverse();
        return path;
    }
    
    private CellData GetFromCellMapByPos(Vector3Int pos)
    {
        CellData c;
        cellMap.TryGetValue(pos, out c);
        return c;
    }
    
    private CellData GetRandomCell(int depth, Vector3Int origin)//TODO: optimize restrict this to a smaller area
    {
        int cntr = 0;
        
        while (cntr <= 100)
        {
            cntr++;
            var randId = Random.Range(0, cellmapList.Length);
            var cell = cellmapList[randId];
            
            if(cell.Value.walkable && Helpers.InRange(cell.Key, origin, depth)) return cell.Value;
        }
        
        Debug.LogError("Could not Find Walkable Random Tile");
        return null;
        
    }
    
    private void Update()
    {
        // if (Input.GetMouseButtonDown(0))
        // {
        //     Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //     pos.z = 0;
        //     var own = gridController.grid.WorldToCell(pos);
        //     if(DEBUG) gridController.ColorTileAtCell(own, gridController.mainMap, Color.green);
        //     var target = FindPlayerCellPos();
        //     if(DEBUG) gridController.ColorTileAtCell(target, gridController.mainMap, Color.red);
        //     var path = FindPath(GetFromCellMapByPos(own), GetFromCellMapByPos(target));
        // }
    }
    
    public CellData GetFromCellMapByWorldPos(Vector3 pos)
    {
        CellData c;
        var position = bgGenerator.tilemap.WorldToCell(pos);
        position.z = 0;
        cellMap.TryGetValue(position, out c);
        return c;
    }

    private List<CellData> GetCellNeighbours(CellData cell)
    {
        List<CellData> result = new List<CellData>();
        foreach (var cellPos in ADJACENTCELLS.ALL)
        {
            var c = GetFromCellMapByPos(cell.position + cellPos);
            if(c != null) result.Add(c);
        }

        return result;
    }
  
}
