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
    private bool fullAttackReady = true;

    private AudioController audioController;

    private void Awake()
    {
        WorldGraph.Subscribe(this, typeof(PlayerAttackController));
    }

    private void Start()
    {
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
        if(attackReadyTiming != null) StopCoroutine(attackReadyTiming);
        attackReadyTiming = StartCoroutine(AttackReadyTiming());
    }

    public int CurrentDamage()
    {
        var multi = fullAttackReady ? 1 : 0.1f;
        return Mathf.RoundToInt(equip.current.damage * multi);
    }

    IEnumerator AttackReadyTiming()
    {
        int i;
        for (i = 0; i <= 100; i++)
        {
            yield return new WaitForSeconds(player.settings.chargeSpeed);
            UpdateReadyAttack.Invoke(i);
        }

        i = 100;
        while (Input.GetMouseButton(0))
        {
            i++;
            yield return new WaitForSeconds(0.05f);
            chargingPowerAttack.Invoke(true);
            UpdateReadyAttack.Invoke(i);
        }

        if (i >= 200)
        {
            Debug.Log("Do Power Attack");
        }

        UpdateReadyAttack.Invoke(100);
        chargingPowerAttack.Invoke(false);
        fullAttackReady = true;
    }
}
