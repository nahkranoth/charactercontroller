using UnityEngine;

public class MetaHealthAspectController:MonoBehaviour
{
    public HealthAspectIndicator thirstAspect;
    public HealthAspectIndicator hungerAspect;
    public HealthAspectIndicator sleepAspect;

    private PlayerHealthStatus playerHealthStatus;
    
    private void Start()
    {
        playerHealthStatus = WorldGraph.Retrieve(typeof(PlayerHealthStatus)) as PlayerHealthStatus;
        playerHealthStatus.SetThirst -= OnSetThirst;
        playerHealthStatus.SetThirst += OnSetThirst;
        playerHealthStatus.SetSleep -= OnSetSleep;
        playerHealthStatus.SetSleep += OnSetSleep;
        playerHealthStatus.SetHunger -= OnSetHunger;
        playerHealthStatus.SetHunger += OnSetHunger;
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
