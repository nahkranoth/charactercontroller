using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEntityPlacer : MonoBehaviour
{
    public void GenerateContainer(GameObject obj, Vector3 position)
    {
        var container = Instantiate(obj, transform);
        container.transform.position = position;
    }
}
