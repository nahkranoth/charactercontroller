using System;
using UnityEngine;
using Random = UnityEngine.Random;

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

    public static Vector3Int RandomVector3(int range) //see no Z!!
    {
        return new Vector3Int(Random.Range(-range, range), Random.Range(-range, range), 0);
    }
    
    public static Vector3 RandomVector3F(int range) //see no Z!!
    {
        return new Vector3(Random.Range(-range, range), Random.Range(-range, range), 0);
    }

    public static int CycleConstraint(int max, int min, int index)
    {
        int result = index % (max+1);
        if (result < min) result = max;
        return result;
    }
    
}

public static class Merger
{
    public static T CloneAndMerge<T>(T baseObject, T overrideObject) where T : new()
    {
        var t = typeof(T);
        var publicProperties = t.GetFields();
        
        var output = new T();
        
        foreach (var fieldInfo in publicProperties)
        {
            var overrideValue = fieldInfo.GetValue(overrideObject);
            var defaultValue = !fieldInfo.FieldType.IsValueType 
                ? null 
                : Activator.CreateInstance(fieldInfo.FieldType);

            if (overrideValue == defaultValue)
            {
                fieldInfo.SetValue(output, fieldInfo.GetValue(baseObject));   
            }
            else 
            {
                fieldInfo.SetValue(output, overrideValue);
            }
        }
        
        return output;
    }
}