using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DeepstorageInfo : MonoBehaviour
{
    public Button actionButton;
    public TextMeshProUGUI info;

    public void OnInfo(DeepStorageInfoData data)
    {
        actionButton.gameObject.SetActive(data.onFirstAction != null);
        actionButton.onClick.RemoveAllListeners();
        actionButton.onClick.AddListener(() => data.onFirstAction.action(data.item) );
        actionButton.GetComponentInChildren<TextMeshProUGUI>().text = data.onFirstAction.name;
        info.text = $"{data.item.menuName}\n${data.item.price}";
    }
    
}

