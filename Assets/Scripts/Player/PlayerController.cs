using System.Collections;
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

   public PlayerStatusController statusController;
   
   private Coroutine dodgeRollApplyForce;
   
   public Vector2 Directions
   {
      get => directions;
   }
   
   private void Awake()
   {
      WorldGraph.Subscribe(this, typeof(PlayerController));
      
      PlayerSettings clone = Instantiate(settings);
      statusController.status = clone.status;
      statusController.SetHealth(statusController.status.health);
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
      itemBehaviourController.ChangeHealth -= statusController.ModifyHealth;
      itemBehaviourController.ChangeHealth += statusController.ModifyHealth;
      
      statusController.status.inventory.AddByDescription(itemDescriptions.collection.FindByBehaviours(ItemBehaviourStates.Behaviours.Sword));
      statusController.status.inventory.AddByDescription(itemDescriptions.collection.FindByBehaviours(ItemBehaviourStates.Behaviours.SimpleFood));
      statusController.status.inventory.AddByDescription(itemDescriptions.collection.FindByBehaviours(ItemBehaviourStates.Behaviours.Axe));
      statusController.status.inventory.AddByDescription(itemDescriptions.collection.FindByBehaviours(ItemBehaviourStates.Behaviours.Torch));
      statusController.StatusUpdate();
      equipController.Equip(statusController.status.inventory.FindByBehaviour(ItemBehaviourStates.Behaviours.Sword));
   }

   public void EquipItem(ItemBehaviourStates.Behaviours behaviour)
   {
      var itm = statusController.status.inventory.storage.Find(x => x.behaviour == behaviour);
      equipController.Equip(itm);
   }

   private void OnChargingPowerAttack(bool charging)
   {
      speed = charging ? statusController.status.chargeWalkSpeed : statusController.status.walkSpeed;
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

      speed = statusController.status.walkSpeed;
      if (canRun()) speed = statusController.status.runSpeed;
      
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
         rigid.AddForce(Directions * statusController.status.dodgeRollForce, ForceMode2D.Force);
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
      statusController.ModifyHealth(-damage);
      animator.SetDamage();
      speed = 0;
      invincible = true;
      StartCoroutine(ResetDamageState());
   }

   IEnumerator ResetDamageState()
   {
      yield return new WaitForSeconds(1f);
      invincible = false;
      speed = statusController.status.walkSpeed;
   }

}
