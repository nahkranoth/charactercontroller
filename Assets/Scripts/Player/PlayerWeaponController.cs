using UnityEngine;

public class PlayerWeaponController : MonoBehaviour
{
    public SpriteRenderer weaponSprite;

    public Sprite sword;
    public Sprite axe;
    
    private void Awake()
    {
        WorldGraph.Subscribe(this, typeof(PlayerWeaponController));
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
