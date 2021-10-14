using TMPro;
using UnityEngine;

public class CharacterDebugController : MonoBehaviour
{
    public Transform debugDetectCircle;
    public Transform debugLoseCircle;

    public TextMeshPro stateText;
    // Start is called before the first frame update

    private void Awake()
    {
        WorldGraph.Subscribe(this, typeof(CharacterDebugController));
    }

    public void Init(INPCSettings settings)
    {
        var detect = settings.detectDistance * 2;
        var lose = settings.loseDistance * 2;
        debugDetectCircle.localScale = new Vector3(detect,detect,1);
        debugLoseCircle.localScale = new Vector3(lose,lose,1);
    }

    public void SetStateText(string state)
    {
        stateText.text = state;
    }

}
