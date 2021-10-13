using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class BinarySpaceTree
{
    public static Bounds[] Generate(Bounds source, int depth)
    {
        List<BinaryTreeNode> result = new List<BinaryTreeNode>{new BinaryTreeNode{bounds=source}};
        
        for(var j=0;j<depth;j++){
            List<BinaryTreeNode> tempStorage = new List<BinaryTreeNode>();
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
            result = new List<BinaryTreeNode>();
            result.AddRange(tempStorage);
        }

        return result.Select(x => x.bounds).ToArray();
    }

    private static BinaryTreeNode[] SplitHorizontal(BinaryTreeNode node)
    {
        var bounds = node.bounds;
        BinaryTreeNode[] result = new BinaryTreeNode[2];
        Vector3 firstCenter = new Vector3(bounds.center.x, bounds.center.y + bounds.extents.y / 2, 0) ;
        Vector3 size = new Vector3(bounds.size.x, bounds.extents.y, 0) ;
        result[0] = new BinaryTreeNode {bounds = new Bounds(firstCenter, size)};
        Vector3 secondCenter = new Vector3(bounds.center.x, bounds.center.y - bounds.extents.y / 2, 0) ;
        result[1] = new BinaryTreeNode {bounds = new Bounds(secondCenter, size)};
        return result;
    }
    
    private static BinaryTreeNode[] SplitVertical(BinaryTreeNode node)
    {
        var bounds = node.bounds;
        BinaryTreeNode[] result = new BinaryTreeNode[2];
        Vector3 firstCenter = new Vector3(bounds.center.x + bounds.extents.x / 2, bounds.center.y, 0) ;
        Vector3 size = new Vector3(bounds.extents.x, bounds.size.y, 0) ;
        result[0] = new BinaryTreeNode {bounds = new Bounds(firstCenter, size)};
        Vector3 secondCenter = new Vector3(bounds.center.x - bounds.extents.x / 2, bounds.center.y, 0) ;
        result[1] = new BinaryTreeNode {bounds = new Bounds(secondCenter, size)};
        return result;
    }
}
