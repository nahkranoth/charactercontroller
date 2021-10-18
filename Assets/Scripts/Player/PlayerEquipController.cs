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

        worldController.time.OnNightFall -= LightLantern;
        worldController.time.OnNightFall += LightLantern;
        worldController.time.OnDayBreak -= DimLantern;
        worldController.time.OnDayBreak += DimLantern;
        weaponSprite.sprite = sword;
    }

    public void Equip(Item item)
    {
        current = item;
        weaponSprite.sprite = item.equipedSprite;
        DimLantern();
        LightLantern();
    }
    
    private void LightLantern()
    {
        if (current.behaviour == ItemBehaviourStates.Behaviours.Torch)
        {
            lantern.enabled = true;
        }
    }
   
    private void DimLantern()
    {
        lantern.enabled = false;
    }
}
