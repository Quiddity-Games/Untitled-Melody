using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piranha : MonoBehaviour
{
    CircleCollider2D detectionRadius;

    // Start is called before the first frame update
    void Start()
    {
        detectionRadius = this.GetComponentInChildren<CircleCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
