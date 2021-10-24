using UnityEditor.Graphs;
using UnityEngine;

struct DebugDrawSquare
{
    public Color clr;
    
    public Vector3 p1;
    public Vector3 p2;
    public Vector3 p3;
    public Vector3 p4;
    
    public Vector3 p5;
    public Vector3 p6;
    public Vector3 p7;
    public Vector3 p8;
}

public class DebugDrawBounds : MonoBehaviour
{
    private TypedBounds[] bounds;
    public float scale = 1f;
    public Color color;

    private DebugDrawSquare[] drawSquares;
    public bool running;

    private Vector3 position;

    public void SetBounds(TypedBounds[] set, Vector3 pos)
    {
        position = pos;
        bounds = set;
        CalcPositions();
        running = true;
    }

    void Update()
    {
        if (!running) return;
        foreach (var drawSquare in drawSquares)
        {
            Debug.DrawLine(drawSquare.p1, drawSquare.p2, drawSquare.clr);
            Debug.DrawLine(drawSquare.p3, drawSquare.p4, drawSquare.clr);
            Debug.DrawLine(drawSquare.p5, drawSquare.p6, drawSquare.clr);
            Debug.DrawLine(drawSquare.p7, drawSquare.p8, drawSquare.clr);
        }   
    }

    private void CalcPositions()
    {
        drawSquares = new DebugDrawSquare[bounds.Length];
        float margin = 0.96f;
        
        for (var i=0;i < bounds.Length;i++)
        {
            var bound = bounds[i];

            bound.bounds.size = bound.bounds.size * margin;
            
            drawSquares[i].clr = BoundsTypeHelper.GetDebugColor(bound.type);
            drawSquares[i].p1 = (new Vector3(bound.bounds.max.x, bound.bounds.min.y, 1) + position) * scale;
            drawSquares[i].p2 = (new Vector3(bound.bounds.max.x, bound.bounds.max.y, 1) + position) * scale;
            drawSquares[i].p3 = (new Vector3(bound.bounds.min.x, bound.bounds.min.y, 1) + position) * scale;
            drawSquares[i].p4 = (new Vector3(bound.bounds.min.x, bound.bounds.max.y, 1) + position) * scale;
            drawSquares[i].p5 = (new Vector3(bound.bounds.max.x, bound.bounds.min.y, 1) + position) * scale;
            drawSquares[i].p6 = (new Vector3(bound.bounds.min.x, bound.bounds.min.y, 1) + position) * scale;
            drawSquares[i].p7 = (new Vector3(bound.bounds.max.x, bound.bounds.max.y, 1) + position) * scale;
            drawSquares[i].p8 = (new Vector3(bound.bounds.min.x, bound.bounds.max.y, 1) + position) * scale;
        }
    }
}
