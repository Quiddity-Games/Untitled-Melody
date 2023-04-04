using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Responsible for what happens when the player touches a collectable. Attached to collectables themselves.
/// </summary>
public class Collectable : MonoBehaviour
{
    //Previously used to cause collectables to emit a text object displaying how many points the player earned from picking one up; irrelevant while scoring/points functionality is disabled
    /*
    public GameObject collectableScoreDrop;
    GameObject canvas;
    */

    void Start()
    {
        //canvas = GameObject.FindGameObjectWithTag("World Space Canvas");
    }

    /// <summary>
    /// Increments the player's number of acquired collectables, and destroys this collectable.
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            GameManager.instance.numCollected++;

            //Saves information about this collectable to variables that can be used to "undo" the player's acquisition of this collectable should they die/respawn before hitting a new checkpoint
            GameManager.instance.tempNumCollected++;
            GameManager.instance.tempCollectableList.Add(this.gameObject.GetComponent<Transform>().position);

            //Disabling scoring functionality for now
            /*
            GameManager.instance.score += Mathf.RoundToInt(Mathf.Pow(10, 1 + ((GameManager.instance.dashCombos - 1f) / 20f)));
            GameManager.instance.tempScore += Mathf.RoundToInt(Mathf.Pow(10, 1 + ((GameManager.instance.dashCombos - 1f) / 20f)));
            */

            GameManager.instance.collectibleUIText.text = "" + GameManager.instance.numCollected + " / " + GameManager.instance.numCollectables;

            //Disabling scoring functionality for now
            //Instantiate(collectableScoreDrop, this.GetComponent<Transform>().position, Quaternion.identity, canvas.GetComponent<Transform>());

            //Disabling dash combo functionality for now
            //PlayerController.instance.dashCount++;

            Destroy(gameObject);
        }
    }
}
