using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapCollisionDetector : MonoBehaviour
{
    public TilemapCollider2D tilemapCollider;
    public CollisionTilemapGenerator collisionTilemapGenerator;
    private PlayerAttackController playerAttack;
    
    private void Start()
    {
        playerAttack = WorldGraph.Retrieve(typeof(PlayerAttackController)) as PlayerAttackController;
        playerAttack.OnToolHitSomething -= PlayerHitSomething;
        playerAttack.OnToolHitSomething += PlayerHitSomething;
    }

    private void PlayerHitSomething(Collider2D collider, Item tool)
    {
        var pos = tilemapCollider.ClosestPoint(playerAttack.weaponBox.transform.position);
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
