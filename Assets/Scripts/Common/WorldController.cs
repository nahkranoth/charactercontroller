using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class WorldController : MonoBehaviour
{
    public bool menuActive = false;
    public bool npcActive = true;
    public bool playerActive = true;
    public WorldTimeController time;
    private InputController input;
    
    public Action<bool> OnToggleMenu;
    
    private void Awake()
    {
        WorldGraph.Subscribe(this, typeof(WorldController));
    }

    private void Start()
    {
        input = WorldGraph.Retrieve(typeof(InputController)) as InputController;
        input.OpenMenu -= ToggleMenu;
        input.OpenMenu += ToggleMenu;
    }

    public void ToggleMenu()
    {
        menuActive = !menuActive;
        playerActive = !menuActive;
        npcActive = !menuActive;
        OnToggleMenu?.Invoke(menuActive);
    }
    
}
