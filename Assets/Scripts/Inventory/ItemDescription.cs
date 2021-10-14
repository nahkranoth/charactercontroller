using UnityEngine;

[CreateAssetMenu(fileName = "ItemSettings", menuName = "Custom/ItemSettings", order = 1)]
public class ItemDescription : ScriptableObject, IRarity
{
    public Item item;
    public int rarity;
    public int Rarity
    {
        get { return rarity;}
        set { rarity = value;}
    }
}

