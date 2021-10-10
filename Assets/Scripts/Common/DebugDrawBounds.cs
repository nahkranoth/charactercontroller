using UnityEngine;

struct DebugDrawSquare
{
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
    private Bounds[] bounds;
    public float scale = 1f;
    public Color color;

    private DebugDrawSquare[] drawSquares;
    public bool running;

    private Vector3 position;

    public void SetBounds(Bounds[] set, Vector3 pos)
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
            Debug.DrawLine(drawSquare.p1, drawSquare.p2, color);
            Debug.DrawLine(drawSquare.p3, drawSquare.p4, color);
            Debug.DrawLine(drawSquare.p5, drawSquare.p6, color);
            Debug.DrawLine(drawSquare.p7, drawSquare.p8, color);
        }   
    }

    private void CalcPositions()
    {
        drawSquares = new DebugDrawSquare[bounds.Length];
        for (var i=0;i < bounds.Length;i++)
        {
            var bound = bounds[i];
            drawSquares[i].p1 = (new Vector3(bound.max.x, bound.min.y, 1) + position) * scale;
            drawSquares[i].p2 = (new Vector3(bound.max.x, bound.max.y, 1) + position) * scale;
            drawSquares[i].p3 = (new Vector3(bound.min.x, bound.min.y, 1) + position) * scale;
            drawSquares[i].p4 = (new Vector3(bound.min.x, bound.max.y, 1) + position) * scale;
            drawSquares[i].p5 = (new Vector3(bound.max.x, bound.min.y, 1) + position) * scale;
            drawSquares[i].p6 = (new Vector3(bound.min.x, bound.min.y, 1) + position) * scale;
            drawSquares[i].p7 = (new Vector3(bound.max.x, bound.max.y, 1) + position) * scale;
            drawSquares[i].p8 = (new Vector3(bound.min.x, bound.max.y, 1) + position) * scale;
        }
    }
}
