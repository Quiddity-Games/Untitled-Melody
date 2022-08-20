using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Collectible : MonoBehaviour
{
    public static TMP_Text collectibleText;
    public static int numCollected;
    // Start is called before the first frame update
    void Start()
    {
        collectibleText = GameObject.Find("CollectibleText").GetComponent<TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        numCollected++;
        collectibleText.text = "" + numCollected + " / 6";
        Destroy(gameObject);
    }
}
