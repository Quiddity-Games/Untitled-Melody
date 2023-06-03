using System;
using RoboRyanTron.Unite2017.Events;
using TMPro;
using UnityEngine;

public interface ICollectable
{
    void Collect();
}

public class Collectable : MonoBehaviour, ICollectable
{
    [SerializeField] private CollectionSignal OnCollect;

    private Vector3 startingLocation;

    private void Start()
    {
        startingLocation = transform.position;
        OnCollect.Register?.Invoke();
    }

    public void Collect()
    {
        OnCollect.SendCollect?.Invoke(this);
        gameObject.SetActive(false);
        transform.position = startingLocation;
    }

    public void Reset()
    {
        gameObject.SetActive(true);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            ICollectable collectable = this;
            collision.gameObject.GetComponent<Player>()?.Collect(collectable);
        }
    }
}