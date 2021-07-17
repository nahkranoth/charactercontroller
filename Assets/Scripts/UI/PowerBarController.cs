using UnityEngine;
using UnityEngine.UI;

public class PowerBarController : MonoBehaviour
{
    public Text powerReadyTxt;
    private PlayerAttackController playerAttackController;

    private void Start()
    {
        playerAttackController = WorldGraph.Retrieve(typeof(PlayerAttackController)) as PlayerAttackController;
        playerAttackController.UpdateReadyAttack -= UpdatePowerReadyText;
        playerAttackController.UpdateReadyAttack += UpdatePowerReadyText;
    }

    private void UpdatePowerReadyText(int amount)
    {
        powerReadyTxt.text = $"{amount.ToString()}%";
    }
    
}
