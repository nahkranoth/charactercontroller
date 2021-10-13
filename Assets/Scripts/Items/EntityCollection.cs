using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EntityCollection", menuName = "Custom/EntityCollection", order = 4)]
public class EntityCollection : ScriptableObject
{
    public List<GameObject> collection;

    public GameObject GetRandom()
    {
        return collection[Random.Range(0, collection.Count)];
    }
}
