using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class TileConstruct:ScriptableObject
{
    public string name;
    public List<TileWrapper> map;
}
