using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DeepstorageInfo : MonoBehaviour
{
    public Button actionButton;
    public TextMeshProUGUI info;

    public void OnInfo(DeepStorageInfoData data)
    {
        actionButton.gameObject.SetActive(data.isShop);
        actionButton.onClick.RemoveAllListeners();
        actionButton.onClick.AddListener(() => data.onFirstAction(data.item) );
        actionButton.GetComponentInChildren<TextMeshProUGUI>().text = data.firstActionName;
        info.text = $"{data.item.menuName}\n${data.item.price}";
    }
    
}

