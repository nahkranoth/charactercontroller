
using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Profiling;
using UnityEngine;
using Random = UnityEngine.Random;


[CreateAssetMenu(fileName = "GeneratorSetCollection", menuName = "Custom/GeneratorSetCollection")]
public class GeneratorSetCollection:ScriptableObject
{
    public GeneratorSet startCity;
    public List<GeneratorSet> collection;

    private int randomIndex = 0;
    private GeneratorSet resultSet;
    public GeneratorSet GetByStep(int step)
    {
        if (step == 0) return startCity;
        
        var rarCandidates = collection.ToList<IRandomProbability>();
        var res = RaritySelector.GetRandom(rarCandidates) as GeneratorSet;
        return res;
    }
}
