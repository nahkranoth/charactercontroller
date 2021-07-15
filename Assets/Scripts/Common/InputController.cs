using System;
using UnityEngine;

public class InputController : MonoBehaviour
{
    public Action<Vector2> Directions;
    public Action StopDirections;
    public Action AttackSlash;
    
    private int vert, hor = 0;
    private Vector2 directions;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0)) AttackSlash?.Invoke();
    }

    void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.W)) vert = 1;
        if (Input.GetKey(KeyCode.S)) vert = -1;
        if (Input.GetKey(KeyCode.D)) hor = 1;
        if (Input.GetKey(KeyCode.A)) hor = -1;
        directions.x = hor;
        directions.y = vert;
        
        if (directions.magnitude == 0)
        { 
            StopDirections.Invoke();
            return;
        }
        
        Directions?.Invoke(directions);
        vert = 0;
        hor = 0;
        directions.x = 0;
        directions.y = 0;
    }
}
