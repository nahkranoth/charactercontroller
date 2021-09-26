using UnityEngine;

public class DebugDrawBounds : MonoBehaviour
{
    private Bounds bounds;
    public float scale = 1f;
    public Color color;
    
    private Vector3 p1;
    private Vector3 p2;
    private Vector3 p3;
    private Vector3 p4;
    
    private Vector3 p5;
    private Vector3 p6;
    private Vector3 p7;
    private Vector3 p8;

    private void Start()
    {
        CalcPositions();
    }

    public void SetBounds(Bounds set)
    {
        bounds = set;
        CalcPositions();
    }

    void Update()
    {
        Debug.DrawLine(p1, p2, color);
        Debug.DrawLine(p3, p4, color);
        Debug.DrawLine(p5, p6, color);
        Debug.DrawLine(p7, p8, color);
    }

    private void CalcPositions()
    {
 
        p1 = new Vector3(bounds.max.x, bounds.min.y, 1) * scale;
        p2 = new Vector3(bounds.max.x, bounds.max.y, 1) * scale;
        p3 = new Vector3(bounds.min.x, bounds.min.y, 1) * scale;
        p4 = new Vector3(bounds.min.x, bounds.max.y, 1) * scale;
        
        p5 = new Vector3(bounds.max.x, bounds.min.y, 1) * scale;
        p6 = new Vector3(bounds.min.x, bounds.min.y, 1) * scale;
        p7 = new Vector3(bounds.max.x, bounds.max.y, 1) * scale;
        p8 = new Vector3(bounds.min.x, bounds.max.y, 1) * scale;
        
    }
}
