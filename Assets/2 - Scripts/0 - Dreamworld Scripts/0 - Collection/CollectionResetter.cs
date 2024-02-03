using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectionResetter : MonoBehaviour
{
    public void ResetTempCollectables()
    {
        DreamworldEventManager.Instance.CallVoidEvent(DreamworldVoidEventEnum.RESET_TEMP_COLLECT);
        ClearTemp();
    }

    public void ClearTemp()
    {
        DreamworldEventManager.Instance.ResetVoidEvent(DreamworldVoidEventEnum.RESET_TEMP_COLLECT);
    }
}
