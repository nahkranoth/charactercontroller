using UnityEngine;

public class WorldTimeController : MonoBehaviour
{

    public Transform sunLight;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        sunLight.Rotate(Vector3.right, .1f);
    }
}
