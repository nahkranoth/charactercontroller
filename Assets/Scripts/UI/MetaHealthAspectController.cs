using TMPro;
using UnityEngine;

public class MetaHealthAspectController:MonoBehaviour
{
    public HealthAspectIndicator thirstAspect;
    public HealthAspectIndicator hungerAspect;
    public HealthAspectIndicator sleepAspect;

    public TextMeshProUGUI moneyText;
    
    private PlayerController player;
    private PlayerHealthStatus playerHealthStatus;
    private EntityInventory playerInventory;
    
    private void Start()
    {
        player = WorldGraph.Retrieve(typeof(PlayerController)) as PlayerController;
        player.playerHealthStatus.SetThirst -= OnSetThirst;
        player.playerHealthStatus.SetThirst += OnSetThirst;
        player.playerHealthStatus.SetSleep -= OnSetSleep;
        player.playerHealthStatus.SetSleep += OnSetSleep;
        player.playerHealthStatus.SetHunger -= OnSetHunger;
        player.playerHealthStatus.SetHunger += OnSetHunger;

        player.status.OnMoneyChange -= SetMoney;
        player.status.OnMoneyChange += SetMoney;
    }

    private void SetMoney(int amount)
    {
        moneyText.text = $"${amount}";
    }
    
    private void OnSetThirst(float thirst)
    {
        thirstAspect.SetAmount(thirst);
    }

    private void OnSetHunger(float hunger)
    {
        hungerAspect.SetAmount(hunger);
    }

    private void OnSetSleep(float sleep)
    {
        sleepAspect.SetAmount(sleep);
    }
}
