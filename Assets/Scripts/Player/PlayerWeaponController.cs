using UnityEngine;

public class PlayerWeaponController : MonoBehaviour
{
    public SpriteRenderer weaponSprite;

    public Sprite sword;
    public Sprite axe;
    
    
    private void Start()
    {
        weaponSprite.sprite = sword;
    }
}
