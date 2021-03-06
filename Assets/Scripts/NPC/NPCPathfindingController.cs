using System.Collections.Generic;
using UnityEngine;

public class NPCPathfindingController
{
    private int currentNode = 0;
    private List<CellData> path;
    public void InitializePath(List<CellData> path)
    {
        this.path = path;
        currentNode = 0;
    }

    public bool NextNode()
    {
        if (path == null) return false;
        if (currentNode < path.Count-1)
        {
            currentNode++;
            return true;
        }
        return false;
    }
    
    public Vector3 GetTarget()
    {
        if (path == null || path.Count <= currentNode) return Vector3.zero;
        var pathNode = path[currentNode];
        pathNode.worldPos.z = 0;
        return pathNode.worldPos;
    }
    
    public Vector3 FindDeltaVecOfCurrentNode(Vector3 ownPos)
    {
        if (path == null || path.Count <= currentNode) return Vector3.zero;
        var pathNode = path[currentNode];
        pathNode.worldPos.z = 0;
        return pathNode.worldPos - ownPos;
    }
}
