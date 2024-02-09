using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerfectPulse : MonoBehaviour
{

    [SerializeField] private float ttl;

    public void Initialize(float radius, float force)
    {
        this.transform.localScale = new Vector3(radius, radius, 1);
        Collider2D [] enemyColliders = Physics2D.OverlapCircleAll(
            transform.position,
            radius,
            LayerMask.GetMask("Enemy")
        );
        foreach (Collider2D enemy in enemyColliders)
        {
            GameObject enemyGO = enemy.gameObject;
            Rigidbody2D rb = enemyGO.GetComponent<Rigidbody2D>();
            Vector3 repulse = enemyGO.transform.position - transform.position;
            Vector3 repulseDirection = (repulse).normalized;
            float repulseForce = force * Mathf.Max(1, (radius - repulse.magnitude) / radius);
            rb.AddForce(repulseDirection * repulseForce, ForceMode2D.Impulse);
        }

        Collider2D [] puzzleColliders = Physics2D.OverlapCircleAll(
            transform.position,
            radius,
            LayerMask.GetMask("Collectable"));

        foreach (Collider2D puzzle in puzzleColliders)
        {
            ICollectable collectable = puzzle.gameObject.GetComponent<ICollectable>();
            if (collectable != null)
            {
                collectable?.Collect();
            }
        }

        StartCoroutine(Countdown());

    }

    IEnumerator Countdown()
    {
        yield return new WaitForSeconds(ttl);
        Destroy(gameObject);
    }
}
