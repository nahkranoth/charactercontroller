using System;
using System.Collections.Generic;
using UnityEngine;

public class NPCController : MonoBehaviour, ITargetableByEnemy
{
    public NPCAnimatorController animatorController;
    public INPCSettings settings;
    public Rigidbody2D rigidBody;
    public CharacterDebugController charDebug;
    public bool characterDebug;
    public TriggerBox hitTrigger;
    
    public INPCStateNetwork stateNetwork;
    private Dictionary<string, AbstractNPCState> stateDictionary;
    private AbstractNPCState activeState;
    private WorldController worldController;
    [HideInInspector] public PathfindingController pathfinding;
    public NPCPathfindingController npcPathController;
    
    public InteractionHandler handler;

    public ItemCollectionDescription itemCollection;
    public NPCInventory inventory;

    public EntityCollection dropPool;
    
    public NPCHealth myNpcHealth;
    
    [HideInInspector] public bool attacking;
    [HideInInspector] public Transform attackTarget;

    private PlayerController player;
    private PlayerToolController playerTool;
    public MetaLevelEntityPlacer metaEntity;

    private bool initialized;

    public bool triggerOccupied;

    public Action OnDestroyMe;
    
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
        
        handler.OnInteraction -= OnInteraction;
        handler.OnInteraction += OnInteraction;
        
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
        if (attacking && !handler.damageRecovering)
        {
            var dTarget = collider2D.GetComponent<IDamageTarget>();
            if (dTarget == null) return;
            dTarget.Damage(settings.damage);
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
        activeState?.Deactivate();
        activeState = stateDictionary[name];
        activeState.Activate();
        if(characterDebug) charDebug.SetStateText(name);
    }

    private void OnInteraction(int amount, PlayerToolActionType type)
    {
        if (triggerOccupied) return;
        triggerOccupied = true;
        if (type == PlayerToolActionType.Slash) return;
        stateNetwork.OnTriggerByPlayer(type);
    }
    
    public void DestroyMe()
    {
        OnDestroyMe?.Invoke();
    }

    public Transform GetTransform()
    {
        return transform;
    }
}
