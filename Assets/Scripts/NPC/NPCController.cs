using System;
using System.Collections.Generic;
using UnityEngine;

public class NPCController : MonoBehaviour
{
    public NPCAnimatorController animatorController;
    public Collider2D mainCollider;
    public INPCSettings settings;
    public Rigidbody2D rigidBody;
    public CharacterDebugController charDebug;
    public bool characterDebug;
    public TriggerBox hitTrigger;
    
    private INPCStateNetwork stateNetwork;
    private Dictionary<string, AbstractNPCState> stateDictionary;
    private AbstractNPCState activeState;
    private WorldController worldController;
    [HideInInspector] public PathfindingController pathfinding;
    public NPCPathfindingController npcPathController;
    
    public InteractionDamageTaker damageTaker;

    public ItemCollectionDescription itemCollection;
    public NPCInventory inventory;

    public EntityCollection dropPool;
    
    public NPCHealth myNpcHealth;
    
    [HideInInspector] public bool attacking = false;

    private PlayerController player;
    private PlayerAttackController playerAttack;
    private MetaLevelEntityPlacer metaEntity;

    private bool initialized;

    void Start()
    {
        myNpcHealth.Set(settings.GetHealth());

        transform.position = new Vector3(transform.position.x, transform.position.y, 0);
        
        stateNetwork = (INPCStateNetwork)Activator.CreateInstance(settings.GetStateNetworkType());
        
        player = WorldGraph.Retrieve(typeof(PlayerController)) as PlayerController;
        worldController = WorldGraph.Retrieve(typeof(WorldController)) as WorldController;
        metaEntity = WorldGraph.Retrieve(typeof(MetaLevelEntityPlacer)) as MetaLevelEntityPlacer;

        hitTrigger.OnTriggerStay -= OnHitSomething;
        hitTrigger.OnTriggerStay += OnHitSomething;
        
        if(characterDebug) charDebug.Init(settings);
        charDebug.gameObject.SetActive(characterDebug);
        
        pathfinding = WorldGraph.Retrieve(typeof(PathfindingController)) as PathfindingController;
        npcPathController = new NPCPathfindingController();
        
        stateDictionary = stateNetwork.GetStateNetwork(this, settings);
        
        SetState(stateNetwork.GetStartNode());
        charDebug.SetStateText(stateNetwork.GetStartNode());
        
        if(!settings.invincible){
            damageTaker.OnTakeDamage -= Damage;
            damageTaker.OnTakeDamage += Damage;
            damageTaker.OnDamageFinished -= DamageFinished;
            damageTaker.OnDamageFinished += DamageFinished;
        }
        else
        {
            damageTaker.OnTakeDamage -= OnInteraction;
            damageTaker.OnTakeDamage += OnInteraction;
        }

        if (inventory != null)
        {
            inventory.storage.AddByDescription(itemCollection.collection.FindByName("Honey"));
            inventory.storage.AddByDescription(itemCollection.collection.FindByName("Water Bottle"));
            inventory.storage.AddByDescription(itemCollection.collection.FindByBehaviours(ItemBehaviourStates.Behaviours.DonkeySpawner));
        }

        initialized = true;
    }

    private void OnHitSomething(Collider2D collider2D)
    {
        if (attacking && !damageTaker.damageRecovering)//TODO move to stateNetworks
        {
            player.Damage(settings.damage);
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

          //TODO 4 drawn out of my ass (behavior culling)
        if (!settings.distanceCulling || Vector3.Distance(player.transform.position, transform.position) < 4f) 
        {
            activeState.Execute();
        }
        else
        {
            rigidBody.velocity = Vector2.zero;
        }
    }

    public void SetState(string name)
    {
        activeState = stateDictionary[name];
        activeState.Activate();
        if(characterDebug) charDebug.SetStateText(name);
    }

    private void OnInteraction(int amount)
    {
        stateNetwork.OnTriggerByPlayer();
    }

    private void Damage(int amount)
    {
        attacking = false;
        myNpcHealth.Modify(-amount);
        if (myNpcHealth.IsDead())
        {
            Die();
            return;
        }
        SetState(stateNetwork.GetDamagedNode());
        animatorController.Damage();
    }

    private void DamageFinished()
    {
        if(!myNpcHealth.IsDead()) SetState(stateNetwork.GetDamageFinishedNode());
    }

    private void Die()
    {
        StopAllCoroutines();
        SetState(stateNetwork.GetDieNode());
        damageTaker.OnTakeDamage -= Damage;
        Destroy(damageTaker);
    }

    public void Destroy()
    {
        damageTaker.OnDamageFinished -= DamageFinished;
        metaEntity.entityPlacer.GenerateCollectable(dropPool.GetRandom(), transform.localPosition);
        DestroyImmediate(gameObject);
    }
}
