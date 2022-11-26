using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Collectable : MonoBehaviour
{
    public static TMP_Text collectibleText;
    public static int numCollected;
    public static int tempNumCollected;
    public static int numCollectables;
    public static List<Vector3> tempCollectableList;

    // Start is called before the first frame update
    void Start()
    {
        numCollectables++;
        collectibleText = GameObject.Find("CollectibleText").GetComponent<TMP_Text>();
        collectibleText.text = "" + numCollected + " / " + numCollectables;

        tempCollectableList = new List<Vector3>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            numCollected++;
            tempNumCollected++;
            tempCollectableList.Add(this.gameObject.GetComponent<Transform>().position);
            collectibleText.text = "" + numCollected + " / " + numCollectables;
            //PlayerController.instance.dashCount++;
            Destroy(gameObject);
        }
    }
}
