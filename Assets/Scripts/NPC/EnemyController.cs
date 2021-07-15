using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = System.Object;

public class EnemyController : MonoBehaviour
{
    public EnemyAnimatorController animatorController;
    public EnemyDamageController damageController;
    public TriggerBox mainHitbox;
    public INPCSettings settings;
    public Rigidbody2D rigidBody;
    
    [HideInInspector] public Transform damageOrigin;

    private INPCStateNetwork stateNetwork;
    private Dictionary<string, AbstractEnemyState> stateDictionary;
    private AbstractEnemyState activeState;
    [HideInInspector] public PathfindingController pathfinding;
    [HideInInspector] public NPCPathfindingController npcPathController;
    
    private int health = 30;
    private int damage = 5;
    private bool alive = true;
    private bool damageRecovering = false;
    [HideInInspector] public bool attacking = false;

    private PlayerController player;
    
    public int Health
    {
        get
        {
            return health;
        }
    }
    
    void Start()
    {
        health = settings.GetHealth();
        damage = settings.GetDamage();
        
        player = WorldGraph.Retrieve(typeof(PlayerController)) as PlayerController;
        player.attackController.OnWeaponHitSomething -= OnPossibleWeaponHit;
        player.attackController.OnWeaponHitSomething += OnPossibleWeaponHit;
        stateNetwork = (INPCStateNetwork)Activator.CreateInstance(settings.GetStateNetworkType());
        
        pathfinding = WorldGraph.Retrieve(typeof(PathfindingController)) as PathfindingController;;
        npcPathController = new NPCPathfindingController();
        
        stateDictionary = stateNetwork.GetStateNetwork(this, settings);
        
        activeState = stateDictionary[stateNetwork.GetStartNode()];
        activeState.Activate();
        
        mainHitbox.OnTriggerStay -= OnTrigger;
        mainHitbox.OnTriggerStay += OnTrigger;
        
    }

    private void OnPossibleWeaponHit(Collider2D collider, int damage)
    {
        EnemyController target = collider.GetComponent<EnemyController>();
        if (target == this)
        {
            attacking = false;
            Damage(player.transform, damage);
        }
    }

    public void OnTrigger(Collider2D collider)
    {
        PlayerController target = collider.GetComponent<PlayerController>();
        if (target && attacking && !damageRecovering)
        {
            target.Damage(damage);
            attacking = false;
        }
    }
    
    void FixedUpdate()
    {
        if (!alive) return;
        activeState.Execute();
    }

    public void SetState(string name)
    {
        activeState = stateDictionary[name];
        activeState.Activate();
    }

    private void Damage(Transform origin, int amount)
    {
        if (damageRecovering) return;
        damageRecovering = true;
        damageOrigin = origin;
        health -= amount;
        damageController.ShowDamage(amount);
        SetState(stateNetwork.GetDamagedNode());
        animatorController.Damage();
        StartCoroutine(DamageFinished());
    }
    
    IEnumerator DamageFinished()
    {
        if (health <= 0)
        {
            Die();
        }
        yield return new WaitForSeconds(2);
        damageRecovering = false;
        SetState(stateNetwork.GetDamageFinishedNode());
    }

    private void Die()
    {
        gameObject.SetActive(false);
    }
}
