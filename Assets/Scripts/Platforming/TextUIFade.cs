using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TextUIFade : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //this.GetComponent<TMP_Text>().text = Mathf.RoundToInt(Mathf.Pow(10, 1 + ((GameManager.instance.dashCombos - 1f) / 20f))).ToString() + "pts";
        StartCoroutine(FadeAndDestroy());
    }

    void Update()
    {
        this.GetComponent<Transform>().position = new Vector3(this.GetComponent<Transform>().position.x, this.GetComponent<Transform>().position.y + (5 * Time.deltaTime), this.GetComponent<Transform>().position.z);
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
