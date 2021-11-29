﻿using System;
using UnityEngine;

public class PlayerStatusController:MonoBehaviour
{
    public PlayerStatus status;
    
    public Action<int> OnMoneyChange;
    public Action<int> OnChangeHealth;
    public Action OnFullReset;

    private void Awake()
    {
        WorldGraph.Subscribe(this, typeof(PlayerStatusController));
    }
    
    public int Money
    {
        get { return status.money; }
        set { status.money = value; }
    }

    public void ChangeMoney(int amount)
    {
        status.money += amount;
        StatusUpdate();
    }

    public void FullReset()
    {
        OnFullReset?.Invoke();
    }
    
    public void StatusUpdate()
    {
        OnMoneyChange?.Invoke(status.money);
        OnChangeHealth?.Invoke(status.health);
    }

    public int CurrentHealth
    {
        get { return status.health; }
        set { status.health = value; }
    }
    
    public int MaxHealth
    {
        get { return status.modifiers.maxHealth; }
        set { status.modifiers.maxHealth = value; }
    }
    
    public void ModifyHealth(int change)
    {
        status.health += change;
        if (status.health <= 0)
        {
            status.alive = false;
        }
        if (status.health > status.modifiers.maxHealth) status.health = status.modifiers.maxHealth;
        if (status.alive) OnChangeHealth?.Invoke(status.health);
    }

    public void SetHealth(int set)
    {
        status.health = set;
        status.modifiers.maxHealth = set;
        status.alive = true;
        if (status.health <= 0) status.alive = false;
    }

    public bool IsDead()
    {
        return !status.alive;
    }

    public void OverrideStatus(PlayerStatus overStatus)
    {
        status = Merger.CloneAndMerge(status, overStatus);
        FullReset();
        StatusUpdate();
    }

    public bool HasCarrySpace(float weight)
    {
        return status.modifiers.maxCarryWeight > status.inventory.TotalItemWeight() + weight;
    }
}