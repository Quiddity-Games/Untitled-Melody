using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PiranhaDetectionRadius : MonoBehaviour
{
    public float speed;

    private Transform parentGameObjectTransform;
    private GameObject player;
    private Vector2 playerPos;

    // Start is called before the first frame update
    void Start()
    {
        parentGameObjectTransform = this.gameObject.GetComponent<Transform>().parent;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            player = collision.gameObject;
            parentGameObjectTransform.gameObject.GetComponent<SpriteRenderer>().color = new Color(255, 165, 0);
        }

        if(player != null)
        {
            playerPos = player.GetComponent<Transform>().position;
            parentGameObjectTransform.position = Vector2.MoveTowards(parentGameObjectTransform.position, playerPos, speed * Time.deltaTime);
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        player = null;
        parentGameObjectTransform.gameObject.GetComponent<SpriteRenderer>().color = Color.white;


    }
}
