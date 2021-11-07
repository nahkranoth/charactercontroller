using System;
using System.Collections.Generic;

public enum InputType{
    None,
    OpenWheelMenu,
    OpenInventory,
    Select,
    UseTool,
    North,
    South,
    East,
    West,
    Run
}

public class InputCheckData
{
    public InputType type;
    public Func<int, bool> criteria;
    public Action action;
}
