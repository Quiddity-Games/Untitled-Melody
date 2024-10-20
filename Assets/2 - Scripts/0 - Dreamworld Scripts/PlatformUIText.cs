using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlatformUIText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI m_welcomeText;

    [SerializeField] private string m_desktopText;

    [SerializeField] private string m_mobileText;

    [SerializeField] DebugPlatformObj debugPlatform;


    void Start()
    {
        bool isMobile = false;
        #if UNITY_EDITOR
            isMobile = debugPlatform.simulateMobile;
        #elif UNITY_STANDALONE || UNITY_EDITOR
            isMobile = false;
        #elif UNITY_ANDROID || UNITY_IOS
            isMobile = true;
        #endif

        if(isMobile)
        {
            m_welcomeText.text = m_mobileText;
        }
        else
        {
            m_welcomeText.text = m_desktopText;
        }
    }
}
