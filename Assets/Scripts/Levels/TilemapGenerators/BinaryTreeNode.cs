using UnityEngine;

public class BinaryTreeNode
{
    public Bounds bounds;
    public bool isLeaf;

    public BinaryTreeNode()
    {
        isLeaf = Random.Range(0, 2) == 1;
    }
}
