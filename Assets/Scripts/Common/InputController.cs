using System;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    public Action<Vector2> Directions;
    public Action StopDirections;
    public Action UseTool;
    public Action OpenMenu;
    public Action Select;
    public Action OpenDeepStorageAsPlayer;
    
    private int vert, hor = 0;
    private Vector2 directions;

    private List<InputCheckData> keyActions;

    private InputType blockExcept;
    
    private void Awake()
    {
        WorldGraph.Subscribe(this, typeof(InputController));
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
                type = InputType.Select,
                criteria = x => { return Input.GetKeyDown(KeyCode.E); },
                action = () => { Select?.Invoke(); }
            },
            new InputCheckData
            {
                type = InputType.North,
                criteria = x => { return Input.GetKey(KeyCode.W); },
                action = () => { vert = 1; }
            },
            new InputCheckData
            {
                type = InputType.South,
                criteria = x => { return Input.GetKey(KeyCode.S); },
                action = () => { vert = -1; }
            },
            new InputCheckData
            {
                type = InputType.East,
                criteria = x => { return Input.GetKey(KeyCode.D); },
                action = () => { hor = 1; }
            },
            new InputCheckData
            {
                type = InputType.West,
                criteria = x => { return Input.GetKey(KeyCode.A); },
                action = () => { hor = -1; }
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
        foreach (var kAction in keyActions)
        {
            if (blockExcept != InputType.None && blockExcept != kAction.type) continue;
            if (kAction.criteria.Invoke(0)) kAction.action.Invoke();
        }
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
        vert = 0;
        hor = 0;
        directions.x = 0;
        directions.y = 0;
    }
}
