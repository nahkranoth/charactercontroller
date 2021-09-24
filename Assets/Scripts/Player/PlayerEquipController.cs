using UnityEngine;

public class PlayerEquipController : MonoBehaviour
{
    public SpriteRenderer weaponSprite;

    public Sprite sword;
    public Sprite axe;
    
    private void Awake()
    {
        WorldGraph.Subscribe(this, typeof(PlayerEquipController));
    }

    private void Start()
    {
        weaponSprite.sprite = sword;
    }

    public void Equip(Sprite sprite)
    {
        weaponSprite.sprite = sprite;
    }
}
