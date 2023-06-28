using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class CheckpointSignal : ScriptableObject
{
    public VoidVector3Callback OnCheckpointEnter;
}
