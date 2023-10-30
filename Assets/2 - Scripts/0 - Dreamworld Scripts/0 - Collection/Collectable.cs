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

    [SerializeField] private string currentMessage;
    private string[] activeMessages;

    #region Thought Fragment Messages
    private string[] genericMessages = new string[7]
    {
        "These nights are too long",
        "Need to take my time",
        "Do they care about me?",
        "I’m self-conscious",
        "Is anyone there?",
        "I got this... or maybe not",
        "Can’t fake it, can’t make it"

    };

    private string[] level1Messages = new string[14]
    {
        "Tired of conversations",
        "Just have to love myself",
        "Been waiting for a long time",
        "Mystified I’m still walking",
        "Still wandering",
        "Don’t need nobody else",
        "I might cry",
        "Spend my time within a dream",
        "Bring me life",
        "What am I waiting for?",
        "Hoping someone would be so kind",
        "Dime in the wishing well",
        "I’m waiting for you",
        "Stuck in my mind"
    };
    #endregion

    private void Start()
    {
        startingLocation = transform.position;
        OnCollect.Register?.Invoke();

        activeMessages = new string[genericMessages.Length + level1Messages.Length];

        for (int i = 0; i < activeMessages.Length; i++)
        {
            if (i < genericMessages.Length)
            {
                activeMessages[i] = genericMessages[i];
            } else
            {
                activeMessages[i] = level1Messages[i - genericMessages.Length];
            }
        }

        InvokeRepeating(nameof(RandomizeMessage), 0f, 5f);
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

    private void RandomizeMessage()
    {
        int randomIndex = Random.Range(0, activeMessages.Length - 1);

        currentMessage = activeMessages[randomIndex];
    }
}