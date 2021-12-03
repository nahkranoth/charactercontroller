using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "EntityCollection", menuName = "Custom/EntityCollection", order = 4)]
public class EntityCollection : ScriptableObject
{
    public List<EntityChance> collection;

    public GameObject GetRandom()
    {
        var rarCandidates = collection.ToList<IRandomProbability>();
        var res = RaritySelector.GetRandom(rarCandidates) as EntityChance;
        return res.entity;
    }
}
