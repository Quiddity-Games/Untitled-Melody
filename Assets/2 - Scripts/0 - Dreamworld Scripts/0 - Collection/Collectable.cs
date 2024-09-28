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
    private TextMeshPro textDisplay;
    [SerializeField] private string currentMessage;
    private string[] activeMessages;

    #region Thought Fragment Messages
    private string[] genericMessages = new string[]
    {
        "bring me life",
        "still wandering",
        "I might cry",
        "waiting for you",
        "Stuck in my mind",
        "tell the lies",
        "tired",
        "mystified",
        "love myself",
        "Still walking",
        "wishing well",
        "within a dream",
        "compromise",
        "what am I",
        "following",
        "write a page",
        "moonlight",
        "where she goes",
        "dime in",
        "long days",
        "long nights",
        "bitter blue",
        "take my time",
        "Don’t need nobody",
        "tell me right",
        "through hell",
        "don’t have to",
        "play pretend",
        "faking"
    };

    private string[] negativeMessages = new string[]
    {
        "you can’t",
        "why do I exist?",
        "just don’t",
        "falls apart",
        "won’t work",
        "never good",
        "it gets worse",
        "you’re not special",
        "waste of space",
        "shouldn’t even"

    };
    #endregion

    private bool initialize = false;

    private void Start()
    {
        displayRenderer = display.GetComponent<SpriteRenderer>();

        startingLocation = transform.position;
        activeMessages = genericMessages;
        InitializeDisplay();
        DreamworldEventManager.RegisterCollectable?.Invoke();
    }

    private void InitializeDisplay()
    {
        Sprite[] spritesList = CollectionScoreController.Instance.CollectableSprites;
        int randomIndex = UnityEngine.Random.Range(0, spritesList.Length);
        displayRenderer.sprite = spritesList[randomIndex];

        TextMeshPro[] textMeshes = GetComponentsInChildren<TextMeshPro>(true);

        for (int i = 0; i < textMeshes.Length; i++)
        {
            if (i == randomIndex)
            {
                textDisplay = textMeshes[i];
                textMeshes[i].gameObject.SetActive(true);
                //InvokeRepeating(nameof(RandomizeMessage), 0f, 5f);
            } else
            {
                Destroy(textMeshes[i]);
            }
        }

        //DreamworldEventManager.Instance.CallVoidEvent(DreamworldVoidEventEnum.REGISTER_COLLECTABLE);
    }

    public void Collect()
    {
        DreamworldEventManager.OnCollect?.Invoke(this);
        display.SetActive(false);
        textDisplay.gameObject.SetActive(false);
        transform.position = startingLocation;
        collider.enabled = false;

        CancelInvoke(nameof(RandomizeMessage));
    }

    public void ResetDisplay()
    {
        display.SetActive(true);
        textDisplay.gameObject.SetActive(true);
        collider.enabled = true;
        //InvokeRepeating(nameof(RandomizeMessage), 0f, 5f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Collect();
        }
    }

    private void RandomizeMessage()
    {
        //int randomIndex = UnityEngine.Random.Range(0, activeMessages.Length - 1);

        //currentMessage = activeMessages[randomIndex];
        //textDisplay.text = activeMessages[randomIndex];
    }
}