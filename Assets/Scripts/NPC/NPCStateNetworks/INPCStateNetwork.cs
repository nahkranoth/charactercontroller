using System.Collections.Generic;
using UnityEngine;
using Object = System.Object;

public interface INPCStateNetwork
{ 
    Dictionary<string, AbstractEnemyState> GetStateNetwork(NPCController parent, Object rawSettings);

    string GetStartNode();
    string GetDamagedNode();
    string GetDamageFinishedNode();
    string GetDieNode();
    public void OnTriggerByPlayer(Collider2D collider, PlayerController player);

}
