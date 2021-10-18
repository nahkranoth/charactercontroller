using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapCollisionDetector : MonoBehaviour
{
    public Tilemap collisionTilemap;
    public TilemapCollider2D tilemapCollider;
    private PlayerAttackController playerAttack;
    private void Start()
    {
        playerAttack = WorldGraph.Retrieve(typeof(PlayerAttackController)) as PlayerAttackController;
        playerAttack.OnToolHitSomething -= PlayerHitSomething;
        playerAttack.OnToolHitSomething += PlayerHitSomething;
    }

    private void PlayerHitSomething(Collider2D collider, Item tool, int damage)
    {
        var pos = tilemapCollider.ClosestPoint(playerAttack.weaponBox.transform.position);
        var cell = collisionTilemap.WorldToCell(pos);
        cell.z = 0;
        var til = collisionTilemap.GetTile(cell);
        if (tool.canChopWood && cell != null && til != null)
        {
            collisionTilemap.SetTile(cell, null);
        }
    }
    
}
