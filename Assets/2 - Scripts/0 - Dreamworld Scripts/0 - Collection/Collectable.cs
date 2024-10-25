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
    private SpriteRenderer displayRenderer;

    private bool initialize = false;

    [SerializeField] private String lyricDropText;
    private Vector3 collectablePos;

    private void Start()
    {
        displayRenderer = display.GetComponent<SpriteRenderer>();
        startingLocation = transform.position;
        InitializeDisplay();
        DreamworldEventManager.RegisterCollectable?.Invoke();
        collectablePos = this.GetComponent<Transform>().position;
    }

    private void InitializeDisplay()
    {
        Sprite[] spritesList = CollectionScoreController.Instance.CollectableSprites;
        int randomIndex = UnityEngine.Random.Range(0, spritesList.Length);
        displayRenderer.sprite = spritesList[randomIndex];
    }

    public void Collect()
    {
        GameObject drop = LyricObjectPool.SharedInstance.GetPooledLyricTextObject();
        drop.GetComponent<Transform>().position = collectablePos;
        drop.GetComponent<TextMeshPro>().text = lyricDropText;
        drop.SetActive(true);

        DreamworldEventManager.OnCollect?.Invoke(this);
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