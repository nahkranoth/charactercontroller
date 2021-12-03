using System;
using System.Collections;
using UnityEngine;
public class PlayerToolController: MonoBehaviour
{
    public InputController input;
    public TriggerBox weaponBox;
    public PlayerAnimatorController animator;
    public PlayerEquipController equip;
    public PlayerController player;
    
    public Action<int> UpdateReadyAttack;
    public Action<Collider2D, Item, PlayerToolActionType> OnToolHitSomething;
    
    private Coroutine toolUseTiming;
    private Coroutine attackReadyTiming;
    [HideInInspector] public bool fullAttackReady = true;
    [HideInInspector] public int charge;

    private AudioController audioController;
    private PlayerToolActionType currentToolActionType;
    
    private void Awake()
    {
        WorldGraph.Subscribe(this, typeof(PlayerToolController));
    }

    private void Start()
    {
        charge = 100;
        input.SlashTool -= OnUseToolSlash;
        input.SlashTool += OnUseToolSlash;
        input.ApplyTool -= OnUseToolApply;
        input.ApplyTool += OnUseToolApply;
        weaponBox.OnTriggerExit -= OnToolExit;
        weaponBox.OnTriggerExit += OnToolExit;
        audioController = WorldGraph.Retrieve(typeof(AudioController)) as AudioController;
        player = WorldGraph.Retrieve(typeof(PlayerController)) as PlayerController;
    }
    
    private void OnToolTrigger(Collider2D other)
    {
        OnToolHitSomething?.Invoke(other, equip.current, currentToolActionType);
    }
    
    private void OnToolExit(Collider2D other)
    {
    }
   
    private void OnUseToolSlash()
    {
        animator.AttackSlash();
        audioController.PlaySound(AudioController.AudioClipName.PlayerSwing);
        StartToolUse(PlayerToolActionType.Slash);
    }
    
    private void OnUseToolApply()
    {
        animator.AttackSlash();
        audioController.PlaySound(AudioController.AudioClipName.CollectItem);
        StartToolUse(PlayerToolActionType.Apply);
    }

    public void StartToolUse(PlayerToolActionType type)
    {
        if(toolUseTiming != null) StopCoroutine(toolUseTiming);
        toolUseTiming = StartCoroutine(ToolUseTiming(type));
    }

    IEnumerator ToolUseTiming(PlayerToolActionType type)
    {
        yield return new WaitForSeconds(0.1f);
        weaponBox.OnTriggerStay -= OnToolTrigger;
        weaponBox.OnTriggerStay += OnToolTrigger;
        currentToolActionType = type;
        yield return new WaitForSeconds(0.2f);
        weaponBox.OnTriggerStay -= OnToolTrigger;
        currentToolActionType = PlayerToolActionType.None;
        fullAttackReady = false;
        StartChargeFill();
    }

    public void StartChargeFill()
    {
        if(attackReadyTiming != null) StopCoroutine(attackReadyTiming);
        attackReadyTiming = StartCoroutine(AttackDone());
    }

    public int CurrentDamage()
    {
        var multi = fullAttackReady ? 1 : 0.1f;
        return Mathf.RoundToInt(equip.current.damage * multi);
    }

    public void SetCharge(int amount)
    {
        charge = amount;
        UpdateReadyAttack.Invoke(charge);
        StartChargeFill();
    }

    IEnumerator AttackDone()
    {
        var chargeTime = player.statusController.ChargeTime / 100;
        for (charge = 0; charge <= 100; charge++)
        {
            yield return new WaitForSeconds(chargeTime);
            UpdateReadyAttack.Invoke(charge);
        }

        charge = 100;
        UpdateReadyAttack.Invoke(100);
        fullAttackReady = true;
    }
}
