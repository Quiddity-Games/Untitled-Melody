using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectionResetter : MonoBehaviour
{
    
     private List<Collectable> tempCollectables;
    public void RegisterTemp(Collectable collect)
    {
        tempCollectables.Add(collect);
    }

    public void ResetTempCollectables()
    {
        foreach (Collectable collectable in tempCollectables)
        {
            collectable.Reset();
        }

        ClearTemp();
    }

    public void ClearTemp()
    {
        tempCollectables.Clear();
    }
}
