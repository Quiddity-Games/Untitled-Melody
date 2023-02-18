using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DashTutorialText : MonoBehaviour
{
    int numOfSuccessfulDashes;

    void Start()
    {
        numOfSuccessfulDashes = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0) == true
            && BeatTracker.instance.onBeat
            && numOfSuccessfulDashes < 3)
        {
            numOfSuccessfulDashes++;

            //Causes the tutorial text to pulse the same color as the rhythm indicator does when the player gets a dash, to help draw a connection
            this.GetComponent<TMP_Text>().color = Color.yellow;
            StartCoroutine(ReturnToDefaultColor());
        }

        //Removes tutorial text when it's clear that the player has figured out how the mechanic works
        if (numOfSuccessfulDashes >= 3)
        {
            StartCoroutine(FadeAndDestroy());
            this.GetComponent<Transform>().position = new Vector3(this.GetComponent<Transform>().position.x, this.GetComponent<Transform>().position.y + (5 * Time.deltaTime), this.GetComponent<Transform>().position.z);
        }
    }

    private IEnumerator ReturnToDefaultColor()
    {
        Color startColor = this.GetComponent<TMP_Text>().color;

        float t = 0f;

        while(t < 1)
        {
            Color lerpedColor = Color.Lerp(startColor, Color.white, t);
            this.GetComponent<TMP_Text>().color = lerpedColor;
            t += Time.deltaTime;

            yield return 0;
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
