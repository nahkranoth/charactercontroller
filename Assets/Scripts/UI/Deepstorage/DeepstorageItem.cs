using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DeepstorageItem:MonoBehaviour
{
    public Image img;
    public TextMeshProUGUI infoText;

    public Action<Item> OnSelect;

    private Item item;
    
    public void Apply(Item itm)
    {
        item = itm;
        img.sprite = itm.menuSprite;
        infoText.text = $"x{itm.amount} {itm.menuName}";
    }

    public void OnClick()
    {
        OnSelect?.Invoke(item);
    }
}
