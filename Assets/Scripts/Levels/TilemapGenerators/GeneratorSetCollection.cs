
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GeneratorSetCollection", menuName = "Custom/GeneratorSetCollection")]
public class GeneratorSetCollection:ScriptableObject
{
    public List<GeneratorSet> collection;
    public GeneratorSet GetByStep(int step)
    {
        return collection.FindLast(x => x.step <= step);
    }
}
