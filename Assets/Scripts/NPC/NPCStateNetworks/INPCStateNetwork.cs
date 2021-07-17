using System;
using System.Collections.Generic;

public interface INPCStateNetwork
{ 
    Dictionary<string, AbstractEnemyState> GetStateNetwork(EnemyController parent, Object rawSettings);

    string GetStartNode();
    string GetDamagedNode();
    string GetDamageFinishedNode();
    string GetDieNode();

}
