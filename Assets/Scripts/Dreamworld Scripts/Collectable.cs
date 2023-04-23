using TMPro;
using UnityEngine;

public interface ICollectable
{
    void Collect();
}

public class Collectable : MonoBehaviour, ICollectable
{
    private GameManager _gameManager;
    private TMP_Text _collectibleUIText;

    private void Start()
    {
        _gameManager = FindObjectOfType<GameManager>();
        _collectibleUIText = _gameManager?.CollectibleUIText;
    }

    public void Collect()
    {
        _gameManager?.OnCollectableCollected(transform.position);
        gameObject.SetActive(false);
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