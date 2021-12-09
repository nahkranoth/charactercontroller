using TMPro;
using UnityEngine;

public class MetaHealthAspectController:MonoBehaviour
{
    public HealthAspectIndicator thirstAspect;
    public HealthAspectIndicator hungerAspect;
    public HealthAspectIndicator sleepAspect;

    public TextMeshProUGUI moneyText;
    
    private PlayerController player;
    private PlayerBodyStateController playerBodyStateController;
    private EntityInventory playerInventory;
    
    private void Start()
    {
        player = WorldGraph.Retrieve(typeof(PlayerController)) as PlayerController;
        player.playerBodyStateController.SetThirst -= OnSetThirst;
        player.playerBodyStateController.SetThirst += OnSetThirst;
        player.playerBodyStateController.SetSleep -= OnSetSleep;
        player.playerBodyStateController.SetSleep += OnSetSleep;
        player.playerBodyStateController.SetHunger -= OnSetHunger;
        player.playerBodyStateController.SetHunger += OnSetHunger;

        player.stateController.OnMoneyChange -= SetMoney;
        player.stateController.OnMoneyChange += SetMoney;
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
