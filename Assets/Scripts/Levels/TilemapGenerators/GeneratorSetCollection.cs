
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

    private int switchCounter = 0;
    private int switchCounterTick = 2;
    private int randomIndex = 0;
    private GeneratorSet resultSet;
    public GeneratorSet GetByStep(int step)
    {
        if (step == 0) return startCity;
        
        switchCounter++;
        if (switchCounter > switchCounterTick)
        {
            switchCounter = 0;
            switchCounterTick = Random.Range(switchMin, switchMax);
            randomIndex = Random.Range(0, collection.Count);
            resultSet = collection[randomIndex];
            
            if (resultSet.onlyEverOneInGenerator) switchCounterTick = 1;//I want the switchCounter to only fire ones
        }
        return resultSet;
    }
}
