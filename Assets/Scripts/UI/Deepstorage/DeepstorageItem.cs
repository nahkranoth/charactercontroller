using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DeepstorageItem:MonoBehaviour
{
    public Image img;
    public TextMeshProUGUI amount;

    public void Apply(Item itm)
    {
        img.sprite = itm.menuSprite;
        amount.text = $"x{itm.amount}";
    }
}
