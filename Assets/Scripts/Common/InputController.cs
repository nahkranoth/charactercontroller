using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputController : MonoBehaviour
{
    public PlayerInput playerInput;
    
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

    public void OpenWheelMenu()
    {
        OpenMenu?.Invoke();
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
    
    private void Awake()
    {
        WorldGraph.Subscribe(this, typeof(InputController));

        playerInput.onActionTriggered += context => { Debug.Log(context); }; 
        
        keyActions = new List<InputCheckData>()
        {
            new InputCheckData
            {
                type = InputType.OpenWheelMenu,
                criteria = x => { return Input.GetKeyDown(KeyCode.Q); },
                action = () => { OpenMenu?.Invoke(); }
            },
            new InputCheckData
            {
                type = InputType.OpenInventory,
                criteria = x => { return Input.GetKeyDown(KeyCode.R); },
                action = () => { OpenDeepStorageAsPlayer?.Invoke(); }
            },
            new InputCheckData
            {
                type = InputType.UseTool,
                criteria = x => { return Input.GetMouseButtonDown(0); },
                action = () => { UseTool?.Invoke(); }
            },
            new InputCheckData
            {
                type = InputType.UseTool,
                criteria = x => { return Input.GetMouseButtonDown(1); },
                action = () => { DodgeRoll?.Invoke(); }
            },
            new InputCheckData
            {
                type = InputType.Select,
                criteria = x => { return Input.GetKeyDown(KeyCode.E); },
                action = () => { Select?.Invoke(); }
            },
            new InputCheckData
            {
                type = InputType.Run,
                criteria = x => { return Input.GetKeyDown(KeyCode.LeftShift); },
                action = () => { running = true; }
            },
            new InputCheckData
            {
                type = InputType.Run,
                criteria = x => { return Input.GetKeyUp(KeyCode.LeftShift); },
                action = () => { running = false; }
            }
        };
    }

    public void BlockExcept(InputType exception)
    {
        blockExcept = exception;
    }

    public void LiftBlockExcept()
    {
        blockExcept = InputType.None;
    }

    private void Update()
    {
        // foreach (var kAction in keyActions)
        // {
        //     if (blockExcept != InputType.None && blockExcept != kAction.type) continue;
        //     if (kAction.criteria.Invoke(0)) kAction.action.Invoke();
        // }
        //
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
