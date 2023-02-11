using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextUIClick : MonoBehaviour
{
    int numOfClicks;

    void Start()
    {
        numOfClicks = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0) == true)
        {
            numOfClicks++;
        }

        if (numOfClicks >= 3)
        {
            StartCoroutine(FadeAndDestroy());
            this.GetComponent<Transform>().position = new Vector3(this.GetComponent<Transform>().position.x, this.GetComponent<Transform>().position.y + (5 * Time.deltaTime), this.GetComponent<Transform>().position.z);
        }
    }

    private IEnumerator FadeAndDestroy()
    {
        float alpha = this.GetComponent<TMP_Text>().color.a;

        while(alpha >= 0)
        {
            this.GetComponent<TMP_Text>().color = new Color(this.GetComponent<TMP_Text>().color.r, this.GetComponent<TMP_Text>().color.g, this.GetComponent<TMP_Text>().color.b, alpha);
            alpha -= 0.01f;

            yield return 0;
        }

        Destroy(this.gameObject);
    }
}
