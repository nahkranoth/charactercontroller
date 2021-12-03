using System;

[Serializable]
public class BoundsTypeProbability: IRandomProbability
{
    public BoundsType type;
    public float probability;
    public float Probability => probability;
    
}
