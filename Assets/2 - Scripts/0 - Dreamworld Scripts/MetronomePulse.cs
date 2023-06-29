using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MetronomePulse : MonoBehaviour
{

    private float timeToLive;
    private float currLifetime;

    private float maxRadius;

    private Vector3 transformScale;
    private Transform _transform;

   
    // Start is called before the first frame update
    public void Init(float TTL, float maxRadius)
    {
        timeToLive = TTL;
        currLifetime = timeToLive;
        this.maxRadius = maxRadius;
        _transform = transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (currLifetime < 0)
        {
            Destroy(gameObject);
            //gameObject.SetActive(false);

        }
        else
        {
            currLifetime -= Time.deltaTime;
            _transform.localScale = new Vector3(maxRadius * (1-(currLifetime / timeToLive)),
                maxRadius * (1-(currLifetime / timeToLive)), 1);
        }
    }
    
    
}
