using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MetronomePulse : MonoBehaviour
{

    private float timeToLive;
    private float currLifetime;

    private Vector3 max;
    private Vector3 min;

    private Vector3 transformScale;
    private Transform _transform;

   
    // Start is called before the first frame update
    public void Init(float TTL, float minRadius, float maxRadius)
    {
        timeToLive = TTL;
        currLifetime = timeToLive;
        this.max = new Vector3(maxRadius,maxRadius,1);
        _transform = transform;
        this.min = new Vector3(minRadius,minRadius,1);
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
            _transform.localScale = Vector3.Lerp(min, max, 1-(currLifetime / timeToLive));
        }
    }
    
    
}
