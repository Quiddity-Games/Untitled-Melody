using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Causes a piece of UI text to fade away after spawning. Attached to the Fading Message Text prefab.
/// </summary>
public class TextUIFade : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(FadeAndDestroy());
    }

    void Update()
    {
        //Causes the text to float upward slightly
        this.GetComponent<Transform>().position = new Vector3(this.GetComponent<Transform>().position.x, this.GetComponent<Transform>().position.y + (5 * Time.deltaTime), this.GetComponent<Transform>().position.z);
    }

    /// <summary>
    /// Causes the text to gradually fade away, then destroy itself.
    /// </summary>
    /// <returns></returns>
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
