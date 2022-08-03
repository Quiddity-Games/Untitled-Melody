using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bouncer : MonoBehaviour
{
    private Vector3 startPos;

    public float mod;

    public float spdMod;

    public bool waving = false;

    private float rot1;

    private float rot2;

    private float rot3;
    // Use this for initialization
    void Start()
    {
        //mod = Random.Range(.25f, .5f);
        //spdMod = Random.Range(-2f, 2f);
        startPos = transform.position;
        rot1 = Random.Range(-10, 10);
        rot2 = Random.Range(-10, 10);
        rot3 = Random.Range(-10, 10);
    }

    // Update is called once per frame
    void Update()
    {
       // transform.Rotate(rot1 * Time.deltaTime, rot2 * Time.deltaTime, rot3 * Time.deltaTime);
        transform.position = startPos + Vector3.up * Mathf.Sin(Time.time * spdMod) * mod;
    }
}
