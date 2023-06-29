using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectionPulse : MonoBehaviour
{
    [SerializeField] private NoteTracker _noteTracker;

    [SerializeField] private float pulseForce;
    [SerializeField] private float pulseRadius;

    // Start is called before the first frame update
    void Start()
    {
        _noteTracker.HitCallback += Pulse;
    }

    // Update is called once per frame
    void Pulse(NoteTracker.HitInfo info)
    {
        if (info.rating != NoteTracker.BeatRating.PERFECT)
        {
            return;
        }

        Collider2D [] enemyColliders = Physics2D.OverlapCircleAll(
            transform.position,
            15f,
            LayerMask.GetMask("Enemy")
            );
        foreach (Collider2D enemy in enemyColliders)
        {
            GameObject enemyGO = enemy.gameObject;
            Rigidbody2D rb = enemyGO.GetComponent<Rigidbody2D>();
            Vector3 repulse = enemyGO.transform.position - transform.position;
            Vector3 repulseDirection = (repulse).normalized;
            float repulseForce = pulseForce * Mathf.Max(1, (pulseRadius - repulse.magnitude) / pulseRadius);
            rb.AddForce(repulseDirection * repulseForce, ForceMode2D.Impulse);
        }

        Collider2D [] puzzleColliders = Physics2D.OverlapCircleAll(
            transform.position,
            pulseRadius,
            LayerMask.GetMask("Collectable"));

        foreach (Collider2D puzzle in puzzleColliders)
        {
            ICollectable collectable = puzzle.gameObject.GetComponent<ICollectable>();
            if (collectable != null)
            {
                collectable?.Collect();
            }
            else
            {
            }
        }

    }
}
