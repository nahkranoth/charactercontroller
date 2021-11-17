
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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
        
        randomIndex = Random.Range(0, collection.Count);
        resultSet = collection[randomIndex];
        return resultSet;
    }
}
