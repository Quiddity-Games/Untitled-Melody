using UnityEngine.SceneManagement;
using UnityEngine;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
    void Awake()
    {
        DontDestroyOnLoad(this);
        DOTween.Init();
        DOTween.SetTweensCapacity(350, 150);
    }
}
