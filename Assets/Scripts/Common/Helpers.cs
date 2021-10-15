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
    
    public static bool InRange(Vector3Int a, Vector3Int b, int range)
    {
        return Vector3Int.Distance(a, b) < range;
    }

    public static int CycleConstraint(int max, int min, int index)
    {
        int result = index % (max+1);
        if (result < min) result = max;
        return result;
    }
    
}
