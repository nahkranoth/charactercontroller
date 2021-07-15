using UnityEngine;
using Random = UnityEngine.Random;

public class RandomController:MonoBehaviour
{
    public float randomFloat;
    
    private void Awake()
    {
        WorldGraph.Subscribe(this, typeof(RandomController));
    }

    private void FixedUpdate()
    { 
        randomFloat = Random.value;
    }
}
