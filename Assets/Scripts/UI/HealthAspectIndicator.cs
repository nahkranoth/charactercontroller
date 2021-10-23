using UnityEngine;
using UnityEngine.UI;

public class HealthAspectIndicator:MonoBehaviour
{
    private Image img;
    
    public void Start()
    {
        img = GetComponent<Image>();
    }

    public void SetAmount(float nAmount)
    {
        var newColor = img.color;
        newColor.a = nAmount;
        img.color = newColor;
    }
}
