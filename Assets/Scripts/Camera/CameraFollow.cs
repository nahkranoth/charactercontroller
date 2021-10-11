using System;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public PlayerController target;
    private Vector3 targetPos;
    public float offsetZ;
    private float frameOffsetY;
    public float topMargin;
    public float bottomMargin;
    public float cameraYSnap;
    private bool movingUpwards;

    private void Start()
    {
        targetPos = target.transform.position;
        transform.position = target.transform.position;
    }

    void FixedUpdate()
    {
        if (target.Directions.y > 0) movingUpwards = true;
        if (target.Directions.y < 0) movingUpwards = false;
        
        if (movingUpwards)
        {
            frameOffsetY = Mathf.Lerp(targetPos.y, target.transform.position.y + bottomMargin, cameraYSnap);
        }
        else
        {
            frameOffsetY = Mathf.Lerp(targetPos.y, target.transform.position.y - topMargin, cameraYSnap);
        }

        targetPos = new Vector3(target.transform.position.x, frameOffsetY, offsetZ);
        
        //at very end
        transform.position = targetPos;
    }

}
