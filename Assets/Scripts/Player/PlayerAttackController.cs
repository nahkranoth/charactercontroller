using System;
using System.Collections;
using UnityEngine;
public class PlayerAttackController: MonoBehaviour
{
    public InputController input;
    public TriggerBox weaponBox;
    public PlayerAnimatorController animator;
    public PlayerEquipController equip;
    public PlayerController player;
    
    public Action<int> UpdateReadyAttack;
    public Action<bool> chargingPowerAttack;
    public Action<Collider2D, Item> OnToolHitSomething;
    
    private Coroutine attackTiming;
    private Coroutine attackReadyTiming;
    [HideInInspector] public bool fullAttackReady = true;
    [HideInInspector] public int charge;

    private AudioController audioController;

    private void Awake()
    {
        WorldGraph.Subscribe(this, typeof(PlayerAttackController));
    }

    private void Start()
    {
        charge = 100;
        input.UseTool -= OnAttackSlash;
        input.UseTool += OnAttackSlash;
        audioController = WorldGraph.Retrieve(typeof(AudioController)) as AudioController;
        player = WorldGraph.Retrieve(typeof(PlayerController)) as PlayerController;
    }
    
    private void OnToolTrigger(Collider2D other)
    {
        OnToolHitSomething?.Invoke(other, equip.current);
    }
   
    private void OnAttackSlash()
    {
        animator.AttackSlash();
        audioController.PlaySound(AudioController.AudioClipName.PlayerSwing);
        StartAttackTiming();
    }

    public void StartAttackTiming()
    {
        if(attackTiming != null) StopCoroutine(attackTiming);
        attackTiming = StartCoroutine(AttackTiming());
    }

    IEnumerator AttackTiming()
    {
        yield return new WaitForSeconds(0.1f);
        weaponBox.OnTriggerStay -= OnToolTrigger;
        weaponBox.OnTriggerStay += OnToolTrigger;
        yield return new WaitForSeconds(0.2f);
        weaponBox.OnTriggerStay -= OnToolTrigger;
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
        for (charge = 0; charge <= 100; charge++)
        {
            yield return new WaitForSeconds(player.settings.status.chargeSpeed);
            UpdateReadyAttack.Invoke(charge);
        }

        charge = 100;
        while (Input.GetMouseButton(0))
        {
            if(charge<200) charge++;
            yield return new WaitForSeconds(0.01f);
            chargingPowerAttack.Invoke(true);
            UpdateReadyAttack.Invoke(charge);
        }

        if (charge >= 200)
        {
           Debug.Log("PowerAttack");
        }

        UpdateReadyAttack.Invoke(100);
        chargingPowerAttack.Invoke(false);
        fullAttackReady = true;
    }
}
