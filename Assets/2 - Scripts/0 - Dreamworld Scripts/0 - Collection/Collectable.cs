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
    [SerializeField] private GameObject display;
    [SerializeField] private Collider2D collider;
    private void Start()
    {
        startingLocation = transform.position;
        OnCollect.Register?.Invoke();
    }

    public void Collect()
    {
        OnCollect.SendCollect?.Invoke(this);
        display.SetActive(false);
        transform.position = startingLocation;
        collider.enabled = false;
    }

    public void ResetDisplay()
    {
        display.SetActive(true);
        collider.enabled = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Collect();
        }
    }
}