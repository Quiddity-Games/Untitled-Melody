using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

/// <summary>
/// Makes a the attached gameObject bob up and down to add more visual appeal. Attached to all of the Collectable gameObjects.
/// </summary>
public class Bouncer : MonoBehaviour
{
    [SerializeField] private float _modifier;
    [SerializeField] private float _speedModifier;

    private Vector3 _startPosition;

    void Start()
    {
        _startPosition = transform.position;
    }

    void FixedUpdate()
    {
        //transform.position = _startPos + Vector3.up * Mathf.Sin(Time.time * _spdMod) * _mod;
        float time = Time.time;
        transform.position = _startPosition + Vector3.up * Mathf.Sin(time * _speedModifier) * _modifier;
    }
}
