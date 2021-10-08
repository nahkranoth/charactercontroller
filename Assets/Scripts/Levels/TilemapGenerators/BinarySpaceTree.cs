using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class BinarySpaceTree
{
    public static Bounds[] Generate(Bounds[] seeds, int depth)
    {
        List<Bounds> result = seeds.ToList();
        
        for(var j=0;j<depth;j++){
            List<Bounds> tempStorage = new List<Bounds>();
            for (var i = 0; i < result.Count; i++)
            {
                if (Random.Range(0, 2) == 0)
                {
                    tempStorage.AddRange(SplitVertical(result[i]));
                }
                else
                {
                    tempStorage.AddRange(SplitHorizontal(result[i]));
                }
                tempStorage.Remove(result[i]);
            }
            result = new List<Bounds>();
            result.AddRange(tempStorage);
        }
        
        Debug.Log(result.Count);
        return result.ToArray();
    }

    private static Bounds[] SplitHorizontal(Bounds full)
    {
        Bounds[] result = new Bounds[2];
        Vector3 firstCenter = new Vector3(full.center.x, full.center.y + full.extents.y / 2, 0) ;
        Vector3 size = new Vector3(full.size.x, full.extents.y, 0) ;
        result[0] = new Bounds(firstCenter, size);
        Vector3 secondCenter = new Vector3(full.center.x, full.center.y - full.extents.y / 2, 0) ;
        result[1] = new Bounds(secondCenter, size);
        return result;
    }
    
    private static Bounds[] SplitVertical(Bounds full)
    {
        Bounds[] result = new Bounds[2];
        Vector3 firstCenter = new Vector3(full.center.x + full.extents.x / 2, full.center.y, 0) ;
        Vector3 size = new Vector3(full.extents.x, full.size.y, 0) ;
        result[0] = new Bounds(firstCenter, size);
        Vector3 secondCenter = new Vector3(full.center.x - full.extents.x / 2, full.center.y, 0) ;
        result[1] = new Bounds(secondCenter, size);
        return result;
    }
}
