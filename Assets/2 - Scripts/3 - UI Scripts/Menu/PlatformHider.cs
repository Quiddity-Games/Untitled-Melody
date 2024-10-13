using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformHider : MonoBehaviour
{
    [SerializeField] List<GameObject> m_hideOnMobile;
    [SerializeField] List<GameObject> m_hideOnDesktop;

    [SerializeField] DebugPlatformObj debugPlatform;

    private bool m_toggleMobile = false;
    private bool m_toggleDesktop = false;

    public void Start()
    {
        #if UNITY_STANDALONE
            m_toggleDesktop = true;
            m_toggleMobile = false;
        #elif UNITY_ANDROID || UNITY_IOS
            m_toggleMobile = true;
            m_toggleDesktop = false;
        #endif

        if(m_toggleDesktop && !debugPlatform.simulateMobile)
        {
            foreach(GameObject obj in m_hideOnMobile)
            {
                obj.SetActive(false);
            }
        }
        if(m_toggleMobile || debugPlatform.simulateMobile)
        {
            foreach(GameObject obj in m_hideOnMobile)
            {
                obj.SetActive(false);
            }
        }
    }
}