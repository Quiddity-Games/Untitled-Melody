using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///Creates and keeps track of a set of Collectable Text Drop gameObjects for use when the player touches a Collectable.
/// </summary>
public class LyricObjectPool : MonoBehaviour
{
    public static LyricObjectPool SharedInstance;
    public List<GameObject> pooledLyricTextObjects;    
    [SerializeField] private GameObject lyricTextObjectToPool;
    [SerializeField] private int amountToPool;

    void Awake()
    {
        SharedInstance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        pooledLyricTextObjects = new List<GameObject>();
        GameObject tmp;
        for(int i = 0; i < amountToPool; i++)
        {
            tmp = Instantiate(lyricTextObjectToPool);
            tmp.SetActive(false);
            pooledLyricTextObjects.Add(tmp);
        } 
    }

    public GameObject GetPooledLyricTextObject()
    {
        for(int i = 0; i < amountToPool; i++)
        {
            if(!pooledLyricTextObjects[i].activeInHierarchy)
            {
                return pooledLyricTextObjects[I];
            }
        }
        return null;
    }
}
