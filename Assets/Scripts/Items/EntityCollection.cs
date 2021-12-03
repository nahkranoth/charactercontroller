using System.Collections.Generic;
using Codice.CM.Common.Merge;
using log4net.Filter;
using UnityEngine;

[CreateAssetMenu(fileName = "EntityCollection", menuName = "Custom/EntityCollection", order = 4)]
public class EntityCollection : ScriptableObject
{
    public List<EntityChance> collection;

    public GameObject GetRandom()
    {
        Dictionary<int, float> probMap = new Dictionary<int, float>();
        for (int i = 0; i < collection.Count; i++)
        {
            probMap.Add(i, collection[i].probability);
        }
        var id = WeightedRandom.GetRandom(probMap);
        return collection[id].entity;
    }
}
