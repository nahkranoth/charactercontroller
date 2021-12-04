using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputController : MonoBehaviour
{
    public Action<Vector2> Directions;
    public Action StopDirections;
    public Action SlashTool;
    public Action ToggleWheelMenuShow;
    public Action ApplyTool;
    public Action OpenDeepStorageAsPlayer;
    public Action DodgeRoll;
    public Action OnCloseUI;
    public Action ApplyMenuWheel;
    public Action<int> ApplyMessageBoxAnswer;

    public PlayerInput playerInput;
    
    private float vert, hor = 0;
    private Vector2 directions;

    private List<InputCheckData> keyActions;

    public bool running;
    
    private void Awake()
    {
        WorldGraph.Subscribe(this, typeof(InputController));
    }
    
    public void ChangeScheme(string name)
    {
        playerInput.SwitchCurrentActionMap(name);
    }

    public void CloseUI()
    {
        OnCloseUI?.Invoke();
        //Important! have to do this as coroutine, else I get problems because this is directly called from a inputcontroller
        StartCoroutine(SwitchActionsToPlayer());
    }

    IEnumerator SwitchActionsToPlayer()
    {
        yield return new WaitForEndOfFrame();
        ChangeScheme("Player");
    }
    
    public void OnMove(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            Vector2 inputVec = ctx.ReadValue<Vector2>();
            vert = inputVec.y;
            hor = inputVec.x;
        }

        if (ctx.canceled)
            hor = vert = 0;
    }

    public void SetRun(InputAction.CallbackContext ctx)
    {
        running = ctx.performed;
    }
    
    public void OnDodge(InputAction.CallbackContext ctx)
    {
        if (!ctx.performed) return;
        DodgeRoll?.Invoke();
    }
    
    public void OnAction(InputAction.CallbackContext ctx)
    {
        if (!ctx.performed) return;
        SlashTool?.Invoke();
    }
    
    public void OnUse(InputAction.CallbackContext ctx)
    {
        if (!ctx.performed) return;
        ApplyTool?.Invoke();
    }

    public void OpenPlayerInventory(InputAction.CallbackContext ctx)
    {
        if (!ctx.performed) return;
        OpenDeepStorageAsPlayer?.Invoke();
    }
    
    public void ToggleWheelInventoryShow(InputAction.CallbackContext ctx)
    {
        if (!ctx.performed) return;
        ToggleWheelMenuShow?.Invoke();
    }

    public void OnMenuWheelUse(InputAction.CallbackContext ctx)
    {
        if (!ctx.performed) return;
        ApplyMenuWheel?.Invoke();
    }

    public void OnMessageBoxAnswer(InputAction.CallbackContext ctx)
    {
        if (!ctx.performed) return;
        ApplyMessageBoxAnswer?.Invoke(Int32.Parse(ctx.control.displayName));
    }
    
    void FixedUpdate()
    {
        directions.x = hor;
        directions.y = vert;
        
        if (directions.magnitude == 0)
        { 
            StopDirections?.Invoke();
            return;
        }
        
        Directions?.Invoke(directions);
       
    }
}
