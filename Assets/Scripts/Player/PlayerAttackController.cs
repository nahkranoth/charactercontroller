using System;
using System.Collections;
using UnityEngine;
public class PlayerAttackController: MonoBehaviour
{
    public InputController input;
    public TriggerBox weaponBox;
    public PlayerAnimatorController animator;
    public PlayerEquipController equip;
    
    public Action<int> UpdateReadyAttack;
    public Action<bool> chargingPowerAttack;
    public Action<Collider2D, Item, int> OnToolHitSomething;
    
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
        input.AttackSlash -= OnAttackSlash;
        input.AttackSlash += OnAttackSlash;
        audioController = WorldGraph.Retrieve(typeof(AudioController)) as AudioController;
    }
    
    private void OnToolTrigger(Collider2D other)
    {
        int damage = 1;
        if (fullAttackReady) damage += 5;
        OnToolHitSomething?.Invoke(other, equip.current, damage);
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

    IEnumerator AttackReadyTiming()
    {
        int i;
        for (i = 0; i <= 100; i++)
        {
            yield return new WaitForSeconds(0.01f);
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
