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
            collisionTilemapGenerator.tilemap.SetTile(cell, null);
        }
    }
    
}
