using System;
using System.Collections.Generic;

public interface INPCStateNetwork
{ 
    Dictionary<string, AbstractEnemyState> GetStateNetwork(NPCController parent, Object rawSettings);

    string GetStartNode();
    string GetDamagedNode();
    string GetDamageFinishedNode();
    string GetDieNode();

}
