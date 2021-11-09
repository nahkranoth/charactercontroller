using System;
using UnityEngine;

public class SaveLoad:MonoBehaviour
{
    private PlayerController player;
    
    private void Awake()
    {
        WorldGraph.Subscribe(this, typeof(SaveLoad));
    }

    private void Start()
    {
        player = WorldGraph.Retrieve(typeof(PlayerController)) as PlayerController;
    }

    public void Save()
    {
        string saveData = JsonUtility.ToJson(player.status);
        System.IO.File.WriteAllText(Application.persistentDataPath + "/saveData.json", saveData);
    }
}

