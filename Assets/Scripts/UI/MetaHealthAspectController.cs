using UnityEngine;

public class MetaHealthAspectController:MonoBehaviour
{
    public HealthAspectController thirstAspect;
    public HealthAspectController hungerAspect;
    public HealthAspectController sleepAspect;

    private PlayerHealthStatus playerHealthStatus;
    
    private void Start()
    {
        playerHealthStatus = WorldGraph.Retrieve(typeof(PlayerHealthStatus)) as PlayerHealthStatus;
        playerHealthStatus.DecreaseThirst -= OnDecreaseThirst;
        playerHealthStatus.DecreaseThirst += OnDecreaseThirst;
        playerHealthStatus.DecreaseSleep -= OnDecreaseSleep;
        playerHealthStatus.DecreaseSleep += OnDecreaseSleep;
        playerHealthStatus.DecreaseHunger -= OnDecreaseHunger;
        playerHealthStatus.DecreaseHunger += OnDecreaseHunger;
    }
    
    private void OnDecreaseThirst(float thirst)
    {
        thirstAspect.SetAmount(thirst);
    }

    private void OnDecreaseHunger(float hunger)
    {
        hungerAspect.SetAmount(hunger);
    }

    private void OnDecreaseSleep(float sleep)
    {
        sleepAspect.SetAmount(sleep);
    }
}
