using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MemiMoveSmoother : MonoBehaviour
{


    [SerializeField] private float duration;
    public void Move(Vector3 targetPosition, VoidCallback callback)
    {
        StartCoroutine(LerpPosition(targetPosition, duration, callback));
    }
    IEnumerator LerpPosition(Vector3 targetPosition, float duration, VoidCallback callback)
    {
        float time = 0;
        Vector3 startPosition = transform.position;
        while (time < duration)
        {
            transform.position = Vector3.Lerp(startPosition, targetPosition, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        transform.position = targetPosition;
        callback?.Invoke();
    }
}
