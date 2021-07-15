using UnityEngine;
using UnityEngine.UI;

public class PowerBarController : MonoBehaviour
{
    public Text powerReadyTxt;
    public PlayerAttackController PlayerAttackController;

    private void Start()
    {
        PlayerAttackController.UpdateReadyAttack -= UpdatePowerReadyText;
        PlayerAttackController.UpdateReadyAttack += UpdatePowerReadyText;
    }

    private void UpdatePowerReadyText(int amount)
    {
        powerReadyTxt.text = $"{amount.ToString()}%";
    }
    
}
