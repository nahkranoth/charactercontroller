
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "GeneratorSetCollection", menuName = "Custom/GeneratorSetCollection")]
public class GeneratorSetCollection:ScriptableObject
{
    public List<GeneratorSet> collection;
    public GeneratorSet GetByStep(int step)
    {
        var possible = collection.OrderBy(p => p.step).LastOrDefault(x => x.step <= step);
        
        if (possible == null) possible = collection[0];

        return possible;
    }
}
