using UnityEngine;

[CreateAssetMenu(fileName = "ItemSettings", menuName = "Custom/ItemSettings", order = 1)]
public class ItemDescription : ScriptableObject, IRandomWeight
{
    public Item item;
    public float spawnProbability;
    public float Weight => spawnProbability;
}

