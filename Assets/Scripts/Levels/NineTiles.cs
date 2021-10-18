using System;
using System.Collections.Generic;
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
     *
     * X = empty
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
        SE = 8,
        X = 9
    }
    
    private Dictionary<Matrix4x4, Vector3Int> kernelLibrary = new Dictionary<Matrix4x4, Vector3Int>();

    //Kernel kerneluations

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
    
    public NineTiles()
    {
        // kernelLibrary.Add(kernelOne, new Vector3Int(0,0,5));
       
    }
    public void HandleKernel(Matrix4x4 kernel)
    {
        if (kernel.Equals(kernelOne))
        {
            Debug.Log("Same");
        }
    }

    private Matrix4x4 kernelOne = new Matrix4x4(
        new Vector4(1, 0, 0), 
        new Vector4(1, 1, 0), 
        new Vector4(1, 0, 0), 
        Vector4.zero);
    
    private Matrix4x4 kernelTwo = new Matrix4x4(
        new Vector4(1, 1, 0), 
        new Vector4(1, 1, 0), 
        new Vector4(1, 0, 0), 
        Vector4.zero);
    
    private Matrix4x4 kernelThree = new Matrix4x4(
        new Vector4(1, 1, 0), 
        new Vector4(1, 1, 0), 
        new Vector4(1, 0, 0), 
        Vector4.zero);
    
    private Matrix4x4 kernelFour = new Matrix4x4(
        new Vector4(1, 0, 0), 
        new Vector4(1, 1, 0), 
        new Vector4(1, 1, 0), 
        Vector4.zero);
    
    private Matrix4x4 kernelFive = new Matrix4x4(
        new Vector4(1, 1, 0), 
        new Vector4(1, 1, 0), 
        new Vector4(1, 1, 0), 
        Vector4.zero);
    
    private Matrix4x4 kernelSix = new Matrix4x4(
        new Vector4(1, 1, 1), 
        new Vector4(1, 1, 0), 
        new Vector4(1, 1, 0), 
        Vector4.zero);
    
    private Matrix4x4 kernelSeven = new Matrix4x4(
        new Vector4(1, 0, 0), 
        new Vector4(1, 1, 0), 
        new Vector4(1, 1, 1), 
        Vector4.zero);
    
    private Matrix4x4 kernelEight = new Matrix4x4(
        new Vector4(1, 1, 0), 
        new Vector4(1, 1, 0), 
        new Vector4(1, 1, 1), 
        Vector4.zero);
    
    private Matrix4x4 kernelNine = new Matrix4x4(
        new Vector4(1, 1, 1),
        new Vector4(1, 1, 0), 
        new Vector4(1, 1, 1), 
        Vector4.zero);
   
}
