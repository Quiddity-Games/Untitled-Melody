using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Collectable : MonoBehaviour
{
    public GameObject collectableScoreDrop;
    GameObject canvas;

    void Start()
    {
        canvas = GameObject.FindGameObjectWithTag("World Space Canvas");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            GameManager.instance.numCollected++;
            GameManager.instance.tempNumCollected++;
            GameManager.instance.tempCollectableList.Add(this.gameObject.GetComponent<Transform>().position);

            GameManager.instance.score += Mathf.RoundToInt(Mathf.Pow(10, 1 + ((GameManager.instance.dashCombos - 1f) / 20f)));
            GameManager.instance.tempScore += Mathf.RoundToInt(Mathf.Pow(10, 1 + ((GameManager.instance.dashCombos - 1f) / 20f)));

            GameManager.instance.collectibleText.text = "" + GameManager.instance.numCollected + " / " + GameManager.instance.numCollectables;


            Instantiate(collectableScoreDrop, this.GetComponent<Transform>().position, Quaternion.identity, canvas.GetComponent<Transform>());

            //PlayerController.instance.dashCount++;

            Destroy(gameObject);
        }
    }
}
