using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PiranhaCore : MonoBehaviour
{
    public float speed;
    Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// Used to move the piranha towards the player
    /// </summary>
    /// <param name="destination"></param>
    public void MovePiranhaCore(Vector3 destination)
    {
        float xDistance = destination.x - this.GetComponent<Transform>().position.x;
        float yDistance = destination.y - this.GetComponent<Transform>().position.y;
        float hypotenuseDistance = Vector2.Distance(new Vector2 (destination.x, destination.y), new Vector2(this.GetComponent<Transform>().position.x, this.GetComponent<Transform>().position.y));

        float velocityXComponent = Mathf.Cos(Mathf.Acos(xDistance / hypotenuseDistance));
        float velocityYComponent = Mathf.Sin(Mathf.Asin(yDistance / hypotenuseDistance));

        rb.velocity = new Vector2(velocityXComponent * speed * Time.deltaTime, velocityYComponent * speed * Time.deltaTime);
    }

    private void OnCollisionEnter2D(Collision2D col)
    {

        if(col.gameObject.CompareTag("Player"))
        {
            col.gameObject.transform.position = Checkpoint.currentCheckpoint.transform.position;
        }
    }
}
