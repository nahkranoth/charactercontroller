using UnityEngine;

[CreateAssetMenu(fileName = "ItemSettings", menuName = "Custom/ItemSettings", order = 1)]
public class ItemDescription : ScriptableObject, IRandomProbability
{
    public Item item;
    public float collectableProbability;
    public float Probability => collectableProbability;
}

