using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectionResetter : MonoBehaviour
{
    
     private List<Collectable> tempCollectables;

     private void Start()
     {
         tempCollectables = new List<Collectable>();
     }

     public void RegisterTemp(Collectable collect)
    {
        tempCollectables.Add(collect);
    }

    public void ResetTempCollectables()
    {
        foreach (Collectable collectable in tempCollectables)
        {
            collectable.ResetDisplay();
        }

        ClearTemp();
    }

    public void ClearTemp()
    {
        tempCollectables.Clear();
    }
}
