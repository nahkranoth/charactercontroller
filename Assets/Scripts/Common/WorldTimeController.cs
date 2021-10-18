using System;
using UnityEngine;

public class WorldTimeController : MonoBehaviour
{
    public Transform sunLight;

    public float timeSpeed = 1f;
    public int startTime;
    public int nightfallTime;
    public int dayBreakTime;
    
    private float currentTime;

    public Action OnNightFall;
    public Action OnDayBreak;
    public bool isNight;
    
    // Start is called before the first frame update
    private void Awake()
    {
        WorldGraph.Subscribe(this, typeof(WorldTimeController));
    }

    void Start()
    {
        currentTime = startTime;
        sunLight.Rotate(Vector3.right, startTime);
    }
    
    private float TimeCycled()
    {
        return currentTime % 360;
    }
    
    // Update is called once per frame
    void Update()
    {
        sunLight.Rotate(Vector3.right, timeSpeed*Time.deltaTime);
        currentTime += timeSpeed * Time.deltaTime;
        CheckDayCycle();
    }

    private void CheckDayCycle()
    {
        if (TimeCycled() > nightfallTime && !isNight) 
        {
            isNight = true;
            OnNightFall?.Invoke();
        }
        if (TimeCycled() > dayBreakTime && isNight)
        {
            isNight = false;
            OnDayBreak?.Invoke();
        }
    }
    
   
}
