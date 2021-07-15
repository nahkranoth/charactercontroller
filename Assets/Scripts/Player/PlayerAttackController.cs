using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlayerAttackController: MonoBehaviour
{
    public InputController input;
    public TriggerBox weaponBox;
    public PlayerAnimatorController animator;
    
    public Action<int> UpdateReadyAttack;
    public Action<bool> chargingPowerAttack;
    public Action<Collider2D, int> OnWeaponHitSomething;
    
    private Coroutine attackTiming;
    private Coroutine attackReadyTiming;
    private bool fullAttackReady = true;
    
    private void Start()
    {
        input.AttackSlash -= OnAttackSlash;
        input.AttackSlash += OnAttackSlash;
    }
    
    private void OnWeaponTrigger(Collider2D other)
    {
        int damage = 1;
        if (fullAttackReady) damage += 5;
        OnWeaponHitSomething?.Invoke(other, damage);
    }
   
    private void OnAttackSlash()
    {
        animator.AttackSlash();
        if(attackTiming != null) StopCoroutine(attackTiming);
        attackTiming = StartCoroutine(AttackTiming());
    }

    IEnumerator AttackTiming()
    {
        yield return new WaitForSeconds(0.1f);
        weaponBox.OnTriggerStay -= OnWeaponTrigger;
        weaponBox.OnTriggerStay += OnWeaponTrigger;
        yield return new WaitForSeconds(0.2f);
        weaponBox.OnTriggerStay -= OnWeaponTrigger;
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
