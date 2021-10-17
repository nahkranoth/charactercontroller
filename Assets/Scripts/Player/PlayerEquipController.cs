using UnityEngine;

public class PlayerEquipController : MonoBehaviour
{
    public SpriteRenderer weaponSprite;

    public Item current;
    public Sprite sword;
    
    private void Awake()
    {
        WorldGraph.Subscribe(this, typeof(PlayerEquipController));
    }

    private void Start()
    {
        weaponSprite.sprite = sword;
    }

    public void Equip(Item item)
    {
        current = item;
        weaponSprite.sprite = item.equipedSprite;
    }
}
