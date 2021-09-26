using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    private Vector3 targetPos;
    void Update()
    {
        targetPos = new Vector3(target.position.x, target.position.y, transform.position.z);
        transform.position = targetPos;
    }

}
