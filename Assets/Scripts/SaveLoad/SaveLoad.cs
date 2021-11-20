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
        player.statusController.status.position = player.transform.localPosition;
        string playerSaveData = JsonUtility.ToJson(player.statusController.status);
        System.IO.File.WriteAllText(Application.persistentDataPath + "/saveData.json", playerSaveData);
    }

    public PlayerStatus Load()
    {
        var file = System.IO.File.ReadAllText(Application.persistentDataPath + "/saveData.json");
        var playerStatus = JsonUtility.FromJson<PlayerStatus>(file);
        player.transform.localPosition = playerStatus.position;
        return playerStatus;
    }
}

