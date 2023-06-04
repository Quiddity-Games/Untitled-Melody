using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class CollectionSignal : ScriptableObject
{
    public VoidCollectableCallback SendCollect;
    
    public VoidCallback Register;
}
