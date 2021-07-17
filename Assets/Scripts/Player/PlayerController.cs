using System;
using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
   public InputController input;
   public Rigidbody2D rigid;
   public PlayerAnimatorController animator;
   public PlayerAttackController attackController;
   public Transform spriteHolder;
   public Transform weaponSpriteHolder;
   public PlayerSettings settings;
   private Vector3 myScale = new Vector3(1, 1, 1);

   private AudioController audioController;
   
   private float walkSpeed = 14f;
   private int health = 50;
   private int currentHealth = 50;
   private bool invincible = false;

   public Action<int> OnDamage;

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

   public void Damage(int damage)
   {
      if (invincible) return;
      audioController.PlaySound(AudioController.AudioClipName.PlayerHurt);
      currentHealth -= damage;
      animator.SetDamage();
      OnDamage?.Invoke(damage);
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
