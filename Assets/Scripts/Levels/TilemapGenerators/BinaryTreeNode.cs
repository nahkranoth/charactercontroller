using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class BinaryTreeNode
{
    public Bounds bounds;
    public int depth;
    public int maxDepth;
    
    public BinaryTreeNode left;
    public BinaryTreeNode right;

    public bool IsLeaf
    {
        get{return left == null && right == null;}
    }

    public BinaryTreeNode(Bounds bounds, int depth, int maxDepth)
    {
        this.bounds = bounds;
        this.depth = depth;
        this.maxDepth = maxDepth;
    }
    
    public void Split()
    {
        depth++;
        if (depth >= maxDepth || Random.Range(0f, 1f) <= .2f) return;
        
        if (Random.Range(0, 2) == 0)
        {
            SplitHorizontal();
        }
        else
        {
            SplitVertical();
        }
        left.Split();
        right.Split();
    }
    
    private void SplitHorizontal()
    {
        var rY = Random.Range(20f, bounds.extents.y);
        Vector3 firstSize = new Vector3(bounds.size.x, bounds.extents.y, 0) ;
        Vector3 secondSize = new Vector3(bounds.size.x, bounds.extents.y, 0) ;
        
        Vector3 firstCenter = new Vector3(bounds.center.x, bounds.center.y + firstSize.y / 2, 0);
        Vector3 secondCenter = new Vector3(bounds.center.x, bounds.center.y - secondSize.y / 2, 0);
        
        left = new BinaryTreeNode(new Bounds(firstCenter, firstSize), depth, maxDepth);
        right = new BinaryTreeNode(new Bounds(secondCenter, secondSize), depth, maxDepth);
    }
    
    private void SplitVertical()
    {
        Vector3 firstCenter = new Vector3(bounds.center.x + bounds.extents.x / 2, bounds.center.y, 0) ;
        Vector3 secondCenter = new Vector3(bounds.center.x - bounds.extents.x / 2, bounds.center.y, 0) ;

        Vector3 size = new Vector3(bounds.extents.x, bounds.size.y, 0) ;
        left = new BinaryTreeNode(new Bounds(firstCenter, size), depth, maxDepth);
        right = new BinaryTreeNode(new Bounds(secondCenter, size), depth, maxDepth);
    }
}
