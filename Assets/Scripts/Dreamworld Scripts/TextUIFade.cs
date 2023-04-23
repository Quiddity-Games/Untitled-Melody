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
    private TMP_Text _txt;

    // Start is called before the first frame update
    void Start()
    {
        _txt = GetComponent<TMP_Text>();
        StartCoroutine(FadeAndDestroy());
    }

    void Update()
    {
        //Causes the text to float upward slightly
        transform.position = new Vector3(transform.position.x, transform.position.y + (5 * Time.deltaTime), transform.position.z);
    }

    /// <summary>
    /// Causes the text to gradually fade away, then destroy itself.
    /// </summary>
    /// <returns></returns>
    private IEnumerator FadeAndDestroy()
    {
        float alpha = _txt.color.a;

        while(alpha >= 0)
        {
            _txt.color = new Color(_txt.color.r, _txt.color.g, _txt.color.b, alpha);
            alpha -= 0.01f;

            yield return 0;
        }

        gameObject.SetActive(false);
    }
}
