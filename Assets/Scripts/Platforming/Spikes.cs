using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikes : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        RespawnManager.instance.OnDeath();
        
        //Removed for now, b/c death functionality is being migrated to RespawnManager.cs
        /*
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.transform.position = Checkpoint.currentCheckpoint.transform.position;
        }
        */
    }
}
