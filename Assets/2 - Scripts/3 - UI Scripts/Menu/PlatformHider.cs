using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformHider : MonoBehaviour
{
    [SerializeField] List<GameObject> m_hideOnMobile;
    [SerializeField] List<GameObject> m_hideOnDesktop;

    [SerializeField] DebugPlatformObj debugPlatform;

    private bool m_isMobile = false;

    public void Start()
    {

        #if UNITY_EDITOR
            m_isMobile = debugPlatform.simulateMobile;
        #elif UNITY_STANDALONE || UNITY_EDITOR
            m_isMobile = false;
        #elif UNITY_ANDROID || UNITY_IOS
            m_isMobile = true;
        #endif

        if(!m_isMobile)
        {
            foreach(GameObject obj in m_hideOnDesktop)
            {
                obj.SetActive(false);
            }
        }
        if(m_isMobile)
        {
            foreach(GameObject obj in m_hideOnMobile)
            {
                obj.SetActive(false);
            }
        }
    }
}