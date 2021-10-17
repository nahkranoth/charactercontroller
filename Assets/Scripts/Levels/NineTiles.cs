using System;
using UnityEngine;


[Serializable]
[CreateAssetMenu(fileName = "NineTiles", menuName = "Custom/NineTiles", order = 1)]
public class NineTiles:ScriptableObject
{
    public TileWrapper[] collection = new TileWrapper[9];

    /*
    * [NW][N][NE]
    * [W] [C] [E]
    * [SW][S][SE]
    */
    
    enum NineCollection
    {
        NW = 0,
        N = 1,
        NE = 2,
        W = 3,
        C = 4,
        E = 5,
        SW = 6,
        S = 7,
        SE = 8
    }

    public void HandleKernel(bool[,] kernel)
    {
        
    }

    //Kernel situations

    /*  100
        XX0 = XXE
        100
        
        010
        XX0 = XE0
        100
        
        001
        XX0 = XX[SE]
        100
        
        100
        XX0 = X[NE]0
        010
        
        010
        XX0 = XE0
        010
        
        001
        XX0 = XX[SE]
        010
        
        100
        XX0 = X[NE]0
        001
        
        010
        XX0 = X[E]0
        001
        
        001
        XX0 = X[E]0
        001
        
    */
}
