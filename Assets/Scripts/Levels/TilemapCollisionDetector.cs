using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapCollisionDetector : MonoBehaviour
{
    public TilemapCollider2D tilemapCollider;
    public CollisionTilemapGenerator collisionTilemapGenerator;
    private PlayerToolController playerTool;
    private LevelRepeater levelRepeater;
    private LevelEntityPlacer levelEntityPlacer;
    
    private void Start()
    {
        playerTool = WorldGraph.Retrieve(typeof(PlayerToolController)) as PlayerToolController;
        playerTool.OnToolHitSomething -= PlayerHitSomething;
        playerTool.OnToolHitSomething += PlayerHitSomething;
        levelRepeater = WorldGraph.Retrieve(typeof(LevelRepeater)) as LevelRepeater;
        levelEntityPlacer = WorldGraph.Retrieve(typeof(LevelEntityPlacer)) as LevelEntityPlacer;
    }

    private void PlayerHitSomething(Collider2D collider, Item tool, PlayerToolActionType type)
    {
        var pos = tilemapCollider.ClosestPoint(playerTool.weaponBox.transform.position);
        var cell = collisionTilemapGenerator.tilemap.WorldToCell(pos);
        cell.z = 0;
        var til = collisionTilemapGenerator.tilemap.GetTile(cell);
        if (tool.canChopWood && til != null)
        {
            TileConstruct match = null;
            foreach (var genConstruct in collisionTilemapGenerator.data.generatedConstructs)
            {
                var tempMatch = genConstruct.Item2.ContainsTile(til);
                if (tempMatch != null) match = genConstruct.Item2;
            }

            if (match != null && match.destroyable)
            {
                FloodFill(cell);
                levelEntityPlacer.GenerateCollectable(match.dropOnDestroy.GetRandom().gameObject,collisionTilemapGenerator.tilemap.CellToLocal(cell));
            }
        }
    }

    private void FloodFill(Vector3Int cell)
    {
        if (!collisionTilemapGenerator.tilemap.HasTile(cell)) return;
        collisionTilemapGenerator.tilemap.SetTile(cell, null);
        FloodFill(cell+Vector3Int.up);
        FloodFill(cell+Vector3Int.down);
        FloodFill(cell+Vector3Int.right);
        FloodFill(cell+Vector3Int.left);
    }
    
}
