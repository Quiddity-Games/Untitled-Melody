using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformHider : MonoBehaviour
{
    [SerializeField] List<GameObject> m_hideOnMobile;
    [SerializeField] List<GameObject> m_hideOnDesktop;
    [SerializeField] List<GameObject> m_hideOnWebGL;

    [SerializeField] DebugPlatformObj debugPlatform;


    private bool m_isMobile = false;


    public void Start()
    {

        PlatformEnum platform;
        #if UNITY_EDITOR
            platform = debugPlatform.simulatePlatform;
        #else
            platform = PlatformHelper.GetPlatform();
        #endif

        switch(platform)
        {
            case PlatformEnum.MOBILE:
            {
                foreach(GameObject obj in m_hideOnMobile)
                {
                    obj.SetActive(false);
                }
                break;
            }
            case PlatformEnum.WEBGL:
            {
                foreach(GameObject obj in m_hideOnWebGL)
                {
                    obj.SetActive(false);
                }
                break;
            }
            case PlatformEnum.DESKTOP:
            {
                foreach(GameObject obj in m_hideOnDesktop)
                {
                    obj.SetActive(false);
                }
                break;
            }
        }
    }
}