using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlatformHelper
{
    public static PlatformEnum GetPlatform()
    {
        if(Application.platform == RuntimePlatform.WebGLPlayer)
        {
            return PlatformEnum.WEBGL;
        }
        else if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.Android)
        {
            return PlatformEnum.MOBILE;
        }
        else
        {
            return PlatformEnum.DESKTOP;
        }
    }
}
