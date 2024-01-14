using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScrit : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if(SettingsManager.Instance().GetAccSettings().Occasional)
        {
            Debug.Log("OCCASIONAL ON");
        }
        else
        {
            Debug.Log("OCCASIONAL OFF");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
