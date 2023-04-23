using UnityEngine;

public class Player : MonoBehaviour
{
    public void Collect(ICollectable collectable)
    {
        collectable?.Collect();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Collectable"))
        {
            ICollectable collectable = collision.gameObject.GetComponent<ICollectable>();
            Collect(collectable);
        }
    }
}