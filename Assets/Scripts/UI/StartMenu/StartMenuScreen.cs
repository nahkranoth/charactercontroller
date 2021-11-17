using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartMenuScreen : MonoBehaviour
{

    public Button startButton;
    
    // Start is called before the first frame update
    void Start()
    {
        startButton.onClick.AddListener(OnStart);
    }

    public void OnStart()
    {
        Debug.Log("clickidy");
         SceneManager.LoadScene("InfinityRoad", LoadSceneMode.Single);
    }

}
