using UnityEngine;

public class WorldController : MonoBehaviour
{
    private void Start()
    {
        WorldGraph.Subscribe(this, typeof(WorldController));
    }
}
