using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Collectable : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            GameManager.instance.numCollected++;
            GameManager.instance.tempNumCollected++;
            GameManager.instance.tempCollectableList.Add(this.gameObject.GetComponent<Transform>().position);
            GameManager.instance.collectibleText.text = "" + GameManager.instance.numCollected + " / " + GameManager.instance.numCollectables;
            //PlayerController.instance.dashCount++;
            Destroy(gameObject);
        }
    }
}
