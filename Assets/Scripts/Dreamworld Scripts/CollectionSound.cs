using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class CollectionSound : MonoBehaviour
{
    private AudioSource source;

    public AudioClip collectionSound;

    private void Start()
    {
        source = GetComponent<AudioSource>();
    }

    public void PlaySound()
    {
        source.PlayOneShot(collectionSound);
    }

}
