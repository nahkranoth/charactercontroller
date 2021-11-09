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
   
   private float speed = 14f;
   private Vector2 directions;

   private bool invincible;
   private WorldController worldController;

   public ItemCollectionDescription itemDescriptions;

   public EntityInventory inventory = new EntityInventory();
   public PlayerStatus status;
   
   private Coroutine dodgeRollApplyForce;
   
   public Vector2 Directions
   {
      get => directions;
   }
   
   private void Awake()
   {
      WorldGraph.Subscribe(this, typeof(PlayerController));
      status = settings.status.DeepCopy();
      playerHealthStatus.myHealth.Set(status.health);
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
      inventory.ChangeMoney(300);
      equipController.Equip(inventory.FindByBehaviour(ItemBehaviourStates.Behaviours.Sword));
   }

   public void EquipItem(ItemBehaviourStates.Behaviours behaviour)
   {
      var itm = inventory.storage.Find(x => x.behaviour == behaviour);
      equipController.Equip(itm);
   }

   private void OnChargingPowerAttack(bool charging)
   {
      speed = charging ? status.chargeWalkSpeed : status.walkSpeed;
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

      var speed = status.walkSpeed;
      if (canRun()) speed = status.runSpeed;
      
      rigid.AddForce(directions * speed);
      myScale.x = directions.x == 0 ? 1: directions.x;
      spriteHolder.localScale = myScale;
      weaponSpriteHolder.localScale = myScale;
      animator.SetWalk((int)directions.x,(int)directions.y);
   }

   private bool canRun()
   {
      return input.running && attackController.charge > 0;
   }

   private void DodgeRoll()
   {
      if (attackController.charge < 50) return;
      attackController.SetCharge(attackController.charge - 50);
      animator.DodgeRoll();
      
      if(dodgeRollApplyForce != null) StopCoroutine(dodgeRollApplyForce);
      dodgeRollApplyForce = StartCoroutine(ApplyRollForce());
   }

   private IEnumerator ApplyRollForce()
   {
      var cntr = 0;
      invincible = true;
      while (cntr < 20)
      {
         yield return new WaitForFixedUpdate();
         rigid.AddForce(Directions * status.dodgeRollForce, ForceMode2D.Force);
         cntr++;
      }
      invincible = false;
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
      speed = 0;
      invincible = true;
      StartCoroutine(ResetDamageState());
   }

   IEnumerator ResetDamageState()
   {
      yield return new WaitForSeconds(1f);
      invincible = false;
      speed = status.walkSpeed;
   }

}
