using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls the behavior of -- and is attached to -- the threat radius gameObject of the piranha hazard object.
/// </summary>
[RequireComponent(typeof(AudioSource))]
public class PiranhaDetectionRadius : MonoBehaviour
{
    private GameObject piranhaCore; //The corresponding "core" gameObject associated with this detection radius
    private GameObject player;
    public Vector3 playerPos;
    public AudioClip chaseSound;
    private AudioSource source;
    [SerializeField] private Animator piranhaSpriteAnimator;
    [SerializeField] private Animator piranhaTextAnimator;

    // Start is called before the first frame update
    void Start()
    {
        source = GetComponent<AudioSource>();
        piranhaCore = this.transform.parent.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        //Moves the detection radius with the piranha's core, while still maintaining its own Z-position so it can visually appear "behind" the core
        transform.position = new Vector3 (piranhaCore.transform.position.x, piranhaCore.transform.position.y, this.transform.position.z);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        
        
            if(other.gameObject.CompareTag("Player"))
            {
                source.PlayOneShot(chaseSound);
                piranhaSpriteAnimator.SetTrigger("Chasing Player");
            }
    }

    /// <summary>
    /// Checks if the player has entered the detection radius, and if so, causes the piranha to pursue the player.
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            player = collision.gameObject;
            //piranhaCore.gameObject.GetComponent<SpriteRenderer>().color = new Color(255, 165, 0);   //Visual cue to the player that the piranha has detected them
        }

        //Piranha moves towards the player, but only if the player still exists within its awareness
        if(player != null)
        {
            playerPos = new Vector3(player.GetComponent<Transform>().position.x, player.GetComponent<Transform>().position.y, this.gameObject.GetComponent<Transform>().position.z);

            if (playerPos.x > piranhaCore.transform.position.x)
            {
                piranhaSpriteAnimator.SetBool("Facing Right", true);
                piranhaTextAnimator.SetBool("Facing Right", true);
            } else
            {
                piranhaSpriteAnimator.SetBool("Facing Right", false);
                piranhaTextAnimator.SetBool("Facing Right", false);
            }

            piranhaCore.GetComponent<PiranhaCore>().MovePiranhaCore(playerPos);
        }
    }

    /// <summary>
    /// Resets the piranha's behavior once the player has left its "sight."
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerExit2D(Collider2D collision)
    {

        if (collision.gameObject.tag == "Player")
        {
            player = null;  //Used to prevent the piranha from following the player once they've left the detection radius
            piranhaSpriteAnimator.SetTrigger("Lost Player");
            //piranhaCore.gameObject.GetComponent<SpriteRenderer>().color = Color.white;  //Visual cue that piranha doesn't detect the player anymore

        }
    }
}
