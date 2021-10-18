using UnityEngine;

public class PlayerEquipController : MonoBehaviour
{
    public SpriteRenderer weaponSprite;
    private WorldController worldController;
    public Light lantern;

    public Item current;
    public Sprite sword;
    
    private void Awake()
    {
        WorldGraph.Subscribe(this, typeof(PlayerEquipController));
        lantern.enabled = false;
    }

    private void Start()
    {
        worldController = WorldGraph.Retrieve(typeof(WorldController)) as WorldController;
        weaponSprite.sprite = sword;
    }

    public void Equip(Item item)
    {
        current = item;
        weaponSprite.sprite = item.equipedSprite;
        CheckLantern();
    }
    
    private void CheckLantern()
    {
        lantern.enabled = (current.behaviour == ItemBehaviourStates.Behaviours.Torch);
    }
}
