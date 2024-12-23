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
        PlatformEnum platform;
        #if UNITY_EDITOR
            platform = debugPlatform.simulatePlatform;
        #else
            platform = PlatformHelper.GetPlatform();
        #endif


        if(platform == PlatformEnum.MOBILE)
        {
            m_welcomeText.text = m_mobileText;
        }
        else
        {
            m_welcomeText.text = m_desktopText;
        }
    }
}
