using System;
using System.Collections.Generic;
using UnityEngine;

public class NPCController : MonoBehaviour
{
    public NPCAnimatorController animatorController;
    public TriggerBox mainHitbox;
    public INPCSettings settings;
    public Rigidbody2D rigidBody;
    public CharacterDebugController charDebug;
    public bool characterDebug;
    
    private INPCStateNetwork stateNetwork;
    private Dictionary<string, AbstractEnemyState> stateDictionary;
    private AbstractEnemyState activeState;
    private WorldController worldController;
    [HideInInspector] public PathfindingController pathfinding;
    public NPCPathfindingController npcPathController;
    public InteractionDamageTaker damageTaker;

    public Health myHealth;
    
    private int damage = 5;
    [HideInInspector] public bool attacking = false;

    private PlayerController player;

    private bool initialized = false;
    
    void Start()
    {
        myHealth.Set(settings.GetHealth());
        
        damage = settings.GetDamage();
        
        transform.position = new Vector3(transform.position.x, transform.position.y, 0);
        
        stateNetwork = (INPCStateNetwork)Activator.CreateInstance(settings.GetStateNetworkType());
        
        worldController = WorldGraph.Retrieve(typeof(WorldController)) as WorldController;
        
        if(characterDebug) charDebug.Init(settings);
        charDebug.gameObject.SetActive(characterDebug);
        
        pathfinding = WorldGraph.Retrieve(typeof(PathfindingController)) as PathfindingController;
        npcPathController = new NPCPathfindingController();
        
        stateDictionary = stateNetwork.GetStateNetwork(this, settings);
        
        SetState(stateNetwork.GetStartNode());
        charDebug.SetStateText(stateNetwork.GetStartNode());
        
        mainHitbox.OnTriggerStay -= OnTrigger;
        mainHitbox.OnTriggerStay += OnTrigger;

        damageTaker.OnTakeDamage -= Damage;
        damageTaker.OnTakeDamage += Damage;
        damageTaker.OnDamageFinished -= DamageFinished;
        damageTaker.OnDamageFinished += DamageFinished;

        initialized = true;
    }

    public void OnTrigger(Collider2D collider)
    {
        PlayerController target = collider.GetComponent<PlayerController>();
        if (target && attacking && !damageTaker.damageRecovering)
        {
            target.Damage(damage);
            attacking = false;
        }
    }
    
    void FixedUpdate()
    {
        if (!initialized) return;
        
        if (!worldController.npcActive)
        {
            rigidBody.velocity = Vector2.zero;
            return;
        }
        activeState.Execute();
    }

    public void SetState(string name)
    {
        activeState = stateDictionary[name];
        activeState.Activate();
        if(characterDebug) charDebug.SetStateText(name);
    }

    private void Damage(int amount)
    {
        attacking = false;
        myHealth.Modify(-amount);
        if (myHealth.IsDeath())
        {
            Die();
            return;
        }
        SetState(stateNetwork.GetDamagedNode());
        animatorController.Damage();
    }

    private void DamageFinished()
    {
        if(!myHealth.IsDeath()) SetState(stateNetwork.GetDamageFinishedNode());
    }

    private void Die()
    {
        StopAllCoroutines();
        SetState(stateNetwork.GetDieNode());
        worldController.SpawnChest(transform.position);
    }

    public void Destroy()
    {
        Debug.Log("Destroy Me");
        DestroyImmediate(gameObject);
    }
}
