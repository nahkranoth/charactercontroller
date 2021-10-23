
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "GeneratorSetCollection", menuName = "Custom/GeneratorSetCollection")]
public class GeneratorSetCollection:ScriptableObject
{
    public List<GeneratorSet> collection;
    public GeneratorSet GetByStep(int step)
    {
        return collection.OrderBy(p => p.step).Last(x => x.step <= step);
    }
}
