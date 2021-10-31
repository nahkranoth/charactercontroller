using System;
using UnityEngine;

[Serializable]
public class EntitySpawner
{
    public Vector3Int position;
    public bool npcSpawner;
    public bool collectableSpawner;
    public GameObject entity;
}
