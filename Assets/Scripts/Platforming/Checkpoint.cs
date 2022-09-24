using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    Vector3 levelStart;
    public static GameObject currentCheckpoint;
    ParticleSystem checkPointBurst;
    // Start is called before the first frame update
    void Start()
    {
        checkPointBurst = gameObject.transform.GetChild(0).GetComponent<ParticleSystem>();
        Debug.Log(checkPointBurst.name);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && currentCheckpoint != gameObject)
        {
            currentCheckpoint = gameObject;
            checkPointBurst.Emit(20);
        }
    }
}
