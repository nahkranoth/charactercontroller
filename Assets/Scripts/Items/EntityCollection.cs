using System.Collections.Generic;
using Codice.CM.Common.Merge;
using UnityEngine;

[CreateAssetMenu(fileName = "EntityCollection", menuName = "Custom/EntityCollection", order = 4)]
public class EntityCollection : ScriptableObject
{
    public List<EntityChance> collection;

    public GameObject GetRandom()
    {
        var fullWeight = 0f;
        GameObject selectedEntity = null;
        foreach (var entityC in collection)
        {
            fullWeight += entityC.probability;
        }

        var iR = Random.Range(0, fullWeight);
        
        foreach (EntityChance rc in collection)
        {
            if (iR < rc.probability)
            {
                selectedEntity = rc.entity;
                break;
            }

            iR = iR - rc.probability;
        }

        return selectedEntity;
    }
}
