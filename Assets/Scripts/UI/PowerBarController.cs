using UnityEngine;
using UnityEngine.UI;

public class PowerBarController : MonoBehaviour
{
    public Text powerReadyTxt;
    private PlayerToolController playerToolController;

    private void Start()
    {
        playerToolController = WorldGraph.Retrieve(typeof(PlayerToolController)) as PlayerToolController;
        playerToolController.UpdateReadyAttack -= UpdatePowerReadyText;
        playerToolController.UpdateReadyAttack += UpdatePowerReadyText;
    }

    private void UpdatePowerReadyText(int amount)
    {
        powerReadyTxt.text = $"{amount.ToString()}%";
    }
    
}
