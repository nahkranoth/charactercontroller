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

    public bool timePassing = true;

    private float previousTimeSpeed = 0;
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

    public void SpeedToMorning()
    {
        previousTimeSpeed = timeSpeed;
        timeSpeed = 40f;
    }
    
    private float TimeCycled()
    {
        return currentTime % 360;
    }
    
    // Update is called once per frame
    void Update()
    {
        if (!timePassing) return;
        sunLight.Rotate(Vector3.right, timeSpeed*Time.deltaTime);
        currentTime += timeSpeed * Time.deltaTime;
        CheckDayCycle();
    }

    private void CheckDayCycle()
    {
        if (TimeCycled() > nightfallTime && !isNight) 
        {
            isNight = true;
            sunLight.GetComponent<Light>().shadows = LightShadows.None;
            OnNightFall?.Invoke();
        }
        if (TimeCycled() > dayBreakTime && isNight)
        {
            isNight = false;
            sunLight.GetComponent<Light>().shadows = LightShadows.Soft;
            OnDayBreak?.Invoke();
            timeSpeed = previousTimeSpeed;
        }
    }
    
   
}
