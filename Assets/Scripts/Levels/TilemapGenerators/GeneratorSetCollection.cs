
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

    [Header("Random Generator")]
    public int switchMin;
    public int switchMax;

    private int randomIndex = 0;
    private GeneratorSet resultSet;
    public GeneratorSet GetByStep(int step)
    {
        if (step == 0) return startCity;
        
        //Or Random Event Map
        
        Dictionary<int, float> probMap = new Dictionary<int, float>();
        for (int i = 0; i < collection.Count; i++)
        {
            probMap.Add(i, collection[i].spawnProbability);
        }
        var id = WeightedRandom.GetRandom(probMap);
        resultSet = collection[id];
        return resultSet;
    }
}
