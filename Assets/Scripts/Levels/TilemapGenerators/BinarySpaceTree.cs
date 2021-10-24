using System.Collections.Generic;
using UnityEngine;

public static class BinarySpaceTree
{
    public static TypedBounds[] Generate(Bounds source, int maxDepth)
    {
        var root = new BinaryTreeNode( source, 0, maxDepth);
        root.Split();//start splitting
        //Traverse
        int maxCap = 0;

        List<BinaryTreeNode> workload = new List<BinaryTreeNode>{root};
        List<TypedBounds> result = new List<TypedBounds>();
        while (workload.Count > 0 || maxCap <= 90)
        {
            for (var i = 0; i < workload.Count; i++)
            {
                var node = workload[i];
                if (node.IsLeaf)
                {
                    var rType = BoundsTypeHelper.GetRandomBoundsType();
                    result.Add(new TypedBounds
                    {
                        bounds = node.bounds, 
                        type = rType
                    });
                    workload.Remove(node);
                    continue;
                }
                
                workload.Add(node.left);
                workload.Add(node.right);
                workload.Remove(node);
            }
            maxCap++;
        }
        return result.ToArray();
    }
}
