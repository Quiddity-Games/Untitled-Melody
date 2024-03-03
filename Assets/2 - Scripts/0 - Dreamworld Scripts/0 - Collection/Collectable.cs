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

    private Vector3 startingLocation;
    [SerializeField] private GameObject display;
    [SerializeField] private Collider2D collider;

    private bool initialize = false;
    private void Start()
    {
        startingLocation = transform.position;
    }

    public void Update()
    {
        if (initialize)
        {
            return;
        }

        DreamworldEventManager.Instance.CallVoidEvent(DreamworldVoidEventEnum
            .REGISTER_COLLECTABLE);
        initialize = true;

    }

    public void Collect()
    {
        DreamworldEventManager.Instance.CallVoidEvent(DreamworldVoidEventEnum.COLLECT);
        DreamworldEventManager.Instance.RegisterVoidEventResponse(DreamworldVoidEventEnum.RESET_TEMP_COLLECT, ResetDisplay);
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