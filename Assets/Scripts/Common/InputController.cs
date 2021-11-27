using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputController : MonoBehaviour
{
    public Action<Vector2> Directions;
    public Action StopDirections;
    public Action UseTool;
    public Action OpenMenu;
    public Action Select;
    public Action OpenDeepStorageAsPlayer;
    public Action DodgeRoll;
    
    private float vert, hor = 0;
    private Vector2 directions;

    private List<InputCheckData> keyActions;

    private InputType blockExcept;

    public bool running;

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
        UseTool?.Invoke();
    }
    
    public void OnUse(InputAction.CallbackContext ctx)
    {
        if (!ctx.performed) return;
        Select?.Invoke();
    }

    public void OpenPlayerInventory(InputAction.CallbackContext ctx)
    {
        if (!ctx.performed) return;
        OpenDeepStorageAsPlayer?.Invoke();
    }
    
    public void OpenWheelInventory(InputAction.CallbackContext ctx)
    {
        if (!ctx.performed) return;
        OpenMenu?.Invoke();
    }
    
    private void Awake()
    {
        WorldGraph.Subscribe(this, typeof(InputController));
    }

    public void BlockExcept(InputType exception)
    {
        blockExcept = exception;
    }

    public void LiftBlockExcept()
    {
        blockExcept = InputType.None;
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
