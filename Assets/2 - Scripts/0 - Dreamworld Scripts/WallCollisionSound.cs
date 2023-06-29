using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class WallCollisionSound : MonoBehaviour
{
    private AudioSource source;

    [SerializeField] private AudioClip collision;
    // Start is called before the first frame update
    void Start()
    {
        source = GetComponent<AudioSource>();
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if(col.gameObject.CompareTag("Player"))
        {
            source.PlayOneShot(collision);
        }
    }
}
