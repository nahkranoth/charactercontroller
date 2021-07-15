using UnityEngine;

public static class Helpers
{
    public static bool InRange(float a, float b, float range)
    {
        return a - range <= b && a + range >= b;
    }
    
    public static bool InRange(Vector3 a, Vector3 b, float range)
    {
        return Vector3.Distance(a, b) < range;
    }
}
