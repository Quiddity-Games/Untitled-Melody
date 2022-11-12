using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PiranhaDetectionRadius : MonoBehaviour
{
    //public float speed;
    private GameObject piranhaCore;
    private GameObject player;
    public Vector3 playerPos;

    // Start is called before the first frame update
    void Start()
    {
        piranhaCore = this.transform.parent.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        this.GetComponent<Transform>().position = new Vector3 (piranhaCore.GetComponent<Transform>().position.x, piranhaCore.GetComponent<Transform>().position.y, this.GetComponent<Transform>().position.z);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            player = collision.gameObject;
            piranhaCore.gameObject.GetComponent<SpriteRenderer>().color = new Color(255, 165, 0);
        }

        if(player != null)
        {
            playerPos = new Vector3(player.GetComponent<Transform>().position.x, player.GetComponent<Transform>().position.y, this.gameObject.GetComponent<Transform>().position.z);

            piranhaCore.GetComponent<PiranhaCore>().MovePiranhaCore(playerPos);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        player = null;
        piranhaCore.gameObject.GetComponent<SpriteRenderer>().color = Color.white;
    }
}
