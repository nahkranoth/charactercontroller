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
   public PlayerEquipController equipController;
   public Transform spriteHolder;
   public Transform weaponSpriteHolder;
   public PlayerSettings settings;
   public PlayerHealthStatus playerHealthStatus;
   
   private Vector3 myScale = new Vector3(1, 1, 1);

   private AudioController audioController;
   private ItemBehaviourController itemBehaviourController;
   
   private float walkSpeed = 14f;
   private Vector2 directions;

   private bool invincible;
   private WorldController worldController;

   public ItemCollectionDescription itemDescriptions;

   public EntityInventory inventory = new EntityInventory();
   
   public Vector2 Directions
   {
      get => directions;
      set => directions = value;
   }
   
   private void Awake()
   {
      WorldGraph.Subscribe(this, typeof(PlayerController));
      walkSpeed = settings.walkSpeed;
      playerHealthStatus.myHealth.Set(settings.startHealth);
   }

   private void Start()
   {
      audioController = WorldGraph.Retrieve(typeof(AudioController)) as AudioController;
      worldController = WorldGraph.Retrieve(typeof(WorldController)) as WorldController;
      itemBehaviourController = WorldGraph.Retrieve(typeof(ItemBehaviourController)) as ItemBehaviourController;
      
      input.Directions -= OnDirections;
      input.Directions += OnDirections;
      input.StopDirections -= OnStopDirections;
      input.StopDirections += OnStopDirections;
      input.DodgeRoll += DodgeRoll;
      input.DodgeRoll += DodgeRoll;

      attackController.OnToolHitSomething -= OnToolHitSomething;
      attackController.OnToolHitSomething += OnToolHitSomething;
      attackController.chargingPowerAttack -= OnChargingPowerAttack;
      attackController.chargingPowerAttack += OnChargingPowerAttack;
      itemBehaviourController.Equip -= EquipItem;
      itemBehaviourController.Equip += EquipItem;
      itemBehaviourController.ChangeHealth -= playerHealthStatus.myHealth.Modify;
      itemBehaviourController.ChangeHealth += playerHealthStatus.myHealth.Modify;
      
      inventory.AddByDescription(itemDescriptions.collection.FindByBehaviours(ItemBehaviourStates.Behaviours.Sword));
      inventory.AddByDescription(itemDescriptions.collection.FindByBehaviours(ItemBehaviourStates.Behaviours.Candy));
      inventory.AddByDescription(itemDescriptions.collection.FindByBehaviours(ItemBehaviourStates.Behaviours.Axe));
      inventory.AddByDescription(itemDescriptions.collection.FindByBehaviours(ItemBehaviourStates.Behaviours.Torch));
      inventory.AddByDescription(itemDescriptions.collection.FindByBehaviours(ItemBehaviourStates.Behaviours.WaterBottle));
      inventory.AddByDescription(itemDescriptions.collection.FindByBehaviours(ItemBehaviourStates.Behaviours.Gem));
      inventory.ChangeMoney(300);
      equipController.Equip(inventory.FindByBehaviour(ItemBehaviourStates.Behaviours.Sword));
   }

   public void EquipItem(ItemBehaviourStates.Behaviours behaviour)
   {
      var itm = inventory.storage.Find(x => x.behaviour == behaviour);
      Debug.Log("Equip Item:"+itm.menuName);
      equipController.Equip(itm);
   }

   private void OnChargingPowerAttack(bool charging)
   {
      walkSpeed = charging ? settings.chargeWalkSpeed : settings.walkSpeed;
      animator.SetCharging(charging);
   }
   
   private void OnToolHitSomething(Collider2D collider, Item itm)
   {
      StartCoroutine(ResetDamageState());
   }

   private void OnDirections(Vector2 direct)
   {
      directions = direct;
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

   private void DodgeRoll()
   {
      if (attackController.charge <= 90) return;
      animator.DodgeRoll();
      attackController.SetCharge(0);
      rigid.AddForce(Directions * 2f, ForceMode2D.Impulse);
   }
   
   private void OnStopDirections()
   {
      animator.SetWalk(0, 0);
   }

   public void Damage(int damage)
   {
      if (invincible) return;
      audioController.PlaySound(AudioController.AudioClipName.PlayerHurt);
      playerHealthStatus.myHealth.Modify(-damage);
      animator.SetDamage();
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
