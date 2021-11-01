using System.Collections.Generic;
using UnityEngine;
using Object = System.Object;

public interface INPCStateNetwork
{ 
    Dictionary<string, AbstractNPCState> GetStateNetwork(NPCController parent, Object rawSettings);

    string GetStartNode();
    string GetDamagedNode();
    string GetDamageFinishedNode();
    string GetDieNode();
    public void OnTriggerByPlayer();

}
