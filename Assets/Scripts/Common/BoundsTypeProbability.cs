using System;

[Serializable]
public class BoundsTypeProbability: IRandomWeight
{
    public BoundsType type;
    public float probability;
    public float Weight => probability;
    
}
