using System;
using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour, ITargetableByEnemy, IDamageTarget
{
   public InputController input;
   public Rigidbody2D rigid;
   public PlayerAnimatorController animator;
   public PlayerToolController toolController;
   public PlayerEquipController equipController;
   public Transform spriteHolder;
   public Transform weaponSpriteHolder;
   public PlayerSettings settings;
   
   private Vector3 myScale = new Vector3(1, 1, 1);

   private AudioController audioController;
   private ItemBehaviourController itemBehaviourController;
   
   private float speed = 14f;
   private Vector2 directions;

   private bool invincible;
   private WorldController worldController;

   public ItemCollectionDescription itemDescriptions;

   public PlayerStateController stateController;
   public PlayerBodyStateController playerBodyStateController;
   
   private Coroutine dodgeRollApplyForce;
   
   public Vector2 Directions => directions;

   public EntityInventory Inventory => stateController.status.inventory;
   public EntityInventory Wearing => stateController.status.wearing;

   
   private void Awake()
   {
      WorldGraph.Subscribe(this, typeof(PlayerController));
      
      PlayerSettings clone = Instantiate(settings);
      stateController.status = clone.status;
      stateController.SetHealth(stateController.CurrentHealth);
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

      toolController.OnToolHitSomething -= OnToolHitSomething;
      toolController.OnToolHitSomething += OnToolHitSomething;
      itemBehaviourController.Equip -= EquipItem;
      itemBehaviourController.Equip += EquipItem;
      itemBehaviourController.ChangeHealth -= stateController.ModifyHealth;
      itemBehaviourController.ChangeHealth += stateController.ModifyHealth;
      
      Inventory.AddByDescription(itemDescriptions.collection.FindByBehaviours(ItemBehaviourStates.Behaviours.Sword));
      Inventory.AddByDescription(itemDescriptions.collection.FindByBehaviours(ItemBehaviourStates.Behaviours.SimpleFood));
      // Inventory.AddByDescription(itemDescriptions.collection.FindByBehaviours(ItemBehaviourStates.Behaviours.Axe));
      Inventory.AddByDescription(itemDescriptions.collection.FindByBehaviours(ItemBehaviourStates.Behaviours.Torch));
      Inventory.AddByDescription(itemDescriptions.collection.FindByName("Donkey"));
      
      //Wearables
      // Wearing.AddByDescription(itemDescriptions.collection.FindByName("Shoes"));
      Wearing.AddByDescription(itemDescriptions.collection.FindByName("Backpack"));
      Wearing.AddByDescription(itemDescriptions.collection.FindByName("Amulet"));
      
      stateController.StatusUpdate();
      equipController.Equip(Inventory.FindByBehaviour(ItemBehaviourStates.Behaviours.Sword));
   }

   public void EquipItem(ItemBehaviourStates.Behaviours behaviour)
   {
      var itm = Inventory.storage.Find(x => x.behaviour == behaviour);
      equipController.Equip(itm);
   }
   
   private void OnToolHitSomething(Collider2D collider, Item itm, PlayerToolActionType type)
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

      speed = stateController.WalkSpeed;
      if (canRun()) speed = stateController.RunSpeed;
      
      rigid.AddForce(directions * speed);
      myScale.x = directions.x == 0 ? 1 : Mathf.Sign(directions.x);
      spriteHolder.localScale = myScale;
      weaponSpriteHolder.localScale = myScale;
      animator.SetWalk(Math.Sign(directions.x),Math.Sign(directions.y));
   }

   private bool canRun()
   {
      return input.running && toolController.charge > 0;
   }

   private void DodgeRoll()
   {
      if (toolController.charge < 50) return;
      toolController.SetCharge(toolController.charge - 50);
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
         rigid.AddForce(Directions * stateController.DodgeRollForce, ForceMode2D.Force);
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
      stateController.ModifyHealth(-damage);
      animator.SetDamage();
      speed = 0;
      invincible = true;
      StartCoroutine(ResetDamageState());
   }

   IEnumerator ResetDamageState()
   {
      yield return new WaitForSeconds(1f);
      invincible = false;
      speed = stateController.WalkSpeed;
   }

   public Transform GetTransform()
   {
      return transform;
   }
}
