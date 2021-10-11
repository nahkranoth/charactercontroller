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
    public GridController gridController;
    
    private PlayerController player;
    private Dictionary<Vector3Int, CellData> cellMapTick;
    private Dictionary<Vector3Int, CellData> cellMapTack;
    private Dictionary<Vector3Int, CellData> completeCellMap;
    private LevelRepeater repeater;
    List<CellData> openList = new List<CellData>();
    List<CellData> closeList = new List<CellData>();
    public bool DEBUG = true;

    private void Awake()
    {
        WorldGraph.Subscribe(this, typeof(PathfindingController));
    }

    private void Start()
    {
        repeater = WorldGraph.Retrieve(typeof(LevelRepeater)) as LevelRepeater;
        repeater.OnInit -= Generate;
        repeater.OnInit += Generate;
    }

    public void Generate()
    {
        player = WorldGraph.Retrieve(typeof(PlayerController)) as PlayerController;
        
        cellMapTick = GetTilemapAsCellmap(
            gridController.tickGenerator.background.tilemap,
            gridController.tickGenerator.collision.tilemap,
            gridController.tickGenerator.GetPosition(),
            true
            );
        
        cellMapTack = GetTilemapAsCellmap(
            gridController.tackGenerator.background.tilemap,
            gridController.tackGenerator.collision.tilemap,
            gridController.tackGenerator.GetPositionAsInt(),
            false
            );

        completeCellMap = cellMapTick;
        foreach (var cell in cellMapTack)
        {
            completeCellMap[cell.Key] = cell.Value;
        }
    }

    private Dictionary<Vector3Int, CellData> GetTilemapAsCellmap(Tilemap tilemap, Tilemap collision, Vector3 offsetPos, bool isTick)
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
        var finishCell = GetRandomCell();
        if (startCell != null && finishCell != null)
        {
            return FindPath(startCell, finishCell, depth);
        }
        Debug.LogWarning($"Could not find Path: Start {startCell} Finish {finishCell}");
        return null;
    }
    
    public List<CellData> FindPathToPlayerByWorldPos(Vector3 worldPos, int depth)
    {
        var startCell = GetFromCellMapByWorldPos(worldPos);
        var finishCell = GetFromCellMapByWorldPos(player.transform.position);
        return FindPath(startCell, finishCell, depth);
    }
    public List<CellData> FindPath(CellData startCell, CellData finishCell, int depth=-1)
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
                    return ConstructPath(neighbour, depth);
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
        foreach (var cell in cellMapTick)
        {
            cell.Value.parent = null;
            cell.Value.cost = 0;
            cell.Value.heuristics = 0;
        }
        foreach (var cell in cellMapTack)
        {
            cell.Value.parent = null;
            cell.Value.cost = 0;
            cell.Value.heuristics = 0;
        }
    }
    private List<CellData> ConstructPath(CellData destination, int depth)
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
            if(DEBUG) gridController.ColorTileAtCell(current.position, gridController.tickGenerator.background.tilemap, Color.yellow);
        }

        path.Reverse();
        if(depth != -1 && depth <= path.Count) path = path.GetRange(0, depth);
        return path;
    }
    
    private CellData GetFromCellMapByPos(Vector3Int pos)
    {
        CellData c;
        completeCellMap.TryGetValue(pos, out c);
        return c;
    }
    
    private CellData GetRandomCell()
    {
        int cntr = 0;
        var cellmapList = completeCellMap.ToArray();
        while (true)
        {
            cntr++;
            var randId = Random.Range(0, completeCellMap.Count);
            var cell = cellmapList[randId];
            if(cell.Value.walkable) return cell.Value;
            if (cntr >= 100) break;
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
        var posTick = gridController.tickGenerator.background.tilemap.WorldToCell(pos);
        completeCellMap.TryGetValue(posTick, out c);
        if (c == null)
        {
            var posTack = gridController.tackGenerator.background.tilemap.WorldToCell(pos);
            completeCellMap.TryGetValue(posTack, out c);
        }
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
