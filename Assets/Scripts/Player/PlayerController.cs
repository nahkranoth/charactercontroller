using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
   public InputController input;
   public Rigidbody2D rigid;
   public PlayerAnimatorController animator;
   public PlayerAttackController attackController;
   public PlayerWeaponController weaponController;
   public Transform spriteHolder;
   public Transform weaponSpriteHolder;
   public PlayerSettings settings;
   
   private Vector3 myScale = new Vector3(1, 1, 1);

   private AudioController audioController;
   private ItemBehaviourController itemBehaviourController;
   
   private float walkSpeed = 14f;
   private int health = 50;
   private int currentHealth = 50;
   private bool invincible = false;
   private WorldController worldController;

   public ItemCollectionDescription itemDescriptions;
   public List<Item> items;
   
   public Action<int> OnHealthChange;
   
   public int Health
   {
      get { return health; }
   }
   
   public int CurrentHealth
   {
      get { return currentHealth; }
   }
   private void Awake()
   {
      WorldGraph.Subscribe(this, typeof(PlayerController));
      walkSpeed = settings.walkSpeed;
      currentHealth = settings.startHealth;
      health = settings.startHealth;

   }

   private void Start()
   {
      input.Directions -= OnDirections;
      input.Directions += OnDirections;
      input.StopDirections -= OnStopDirections;
      input.StopDirections += OnStopDirections;
      attackController.OnWeaponHitSomething -= OnWeaponHitSomething;
      attackController.OnWeaponHitSomething += OnWeaponHitSomething;
      attackController.chargingPowerAttack -= OnChargingPowerAttack;
      attackController.chargingPowerAttack += OnChargingPowerAttack;
      
      audioController = WorldGraph.Retrieve(typeof(AudioController)) as AudioController;
      worldController = WorldGraph.Retrieve(typeof(WorldController)) as WorldController;
      itemBehaviourController = WorldGraph.Retrieve(typeof(ItemBehaviourController)) as ItemBehaviourController;

      itemBehaviourController.Equip -= EquipItem;
      itemBehaviourController.Equip += EquipItem;
      
      foreach (var description in itemDescriptions.collection.descriptions)
      {
         items.Add(new Item
         {
            behaviour = description.item.behaviour,
            menuName = description.item.menuName,
            menuSprite = description.item.menuSprite,
            equipedSprite = description.item.equipedSprite,
            consumable = description.item.consumable,
         });
      }
   }

   public void EquipItem(ItemBehaviourStates.Behaviours behaviour)
   {
      var itm = items.Find(x => x.behaviour == behaviour);
      Debug.Log("Equip Item:"+itm.menuName);
      weaponController.Equip(itm.equipedSprite);
   }

   public bool TakeItem(Item item)
   {
      var itm = items.Find(x => x == item);
      if (itm == null) return false;
      itm.amount--;
      if (itm.amount == 0)
      {
         items.Remove(itm);
         return true;//IS EMPTY
      }
      return false;
   }

   private void OnChargingPowerAttack(bool charging)
   {
      walkSpeed = charging ? settings.chargeWalkSpeed : settings.walkSpeed;
      animator.SetCharging(charging);
   }
   
   private void OnWeaponHitSomething(Collider2D collider, int damage)
   {
      StartCoroutine(ResetDamageState());
   }

   private void OnDirections(Vector2 directions)
   {
      if (!worldController.playerActive)
      {
         rigid.velocity = Vector2.zero;
         animator.SetWalk(0,0);
         return;
      }
      
      rigid.AddForce(directions * walkSpeed);
      myScale.x = directions.x == 0 ? 1: directions.x;
      spriteHolder.localScale = myScale;
      weaponSpriteHolder.localScale = myScale;
      animator.SetWalk((int)directions.x,(int)directions.y);
   }
   
   private void OnStopDirections()
   {
      animator.SetWalk(0, 0);
   }


   public void AddHealth(int heal)
   {
      currentHealth += heal;
      OnHealthChange?.Invoke(currentHealth);
   }
   
   public void Damage(int damage)
   {
      if (invincible) return;
      audioController.PlaySound(AudioController.AudioClipName.PlayerHurt);
      currentHealth -= damage;
      animator.SetDamage();
      OnHealthChange?.Invoke(currentHealth);
      walkSpeed = 0;
      invincible = true;
      StartCoroutine(ResetDamageState());
   }

   IEnumerator ResetDamageState()
   {
      yield return new WaitForSeconds(1f);
      invincible = false;
      walkSpeed = settings.walkSpeed;
   }
}
