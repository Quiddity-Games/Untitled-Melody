using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class BackgroundObjectAnimator : MonoBehaviour
{
    private enum AnimationBehaviour { None, Rotate }
    [SerializeField] AnimationBehaviour _behaviour;

    [Space(10)]
    [Header("Animation Duration")]
    [SerializeField] private float _minDuration;
    [SerializeField] private float _maxDuration;
    [SerializeField] private float _currentDuration;
    
    [Header("Angle")]
    [SerializeField] private float _minAngle;
    [SerializeField] private float _maxAngle;
    [SerializeField] private float _currentAngle;



    private Sequence currentSequence;

    // Start is called before the first frame update
    void Start()
    {
        // On start, randomize the duration of the animation and the rotation angle.
        _currentDuration = Random.Range(_minDuration, _maxDuration);
        _currentAngle = Random.Range(_minAngle, _maxAngle);
        StartRotation();
    }

    private void ToggleAnimation()
    {
        if(currentSequence == null)
        {
            StartRotation();    
        }
    }
    

    void StartRotation()
    {
        // If the rotation angle is >= 270, rotate all the way 360 and return to 0. If not, tween back towards 0 the opposite direction.
        currentSequence = DOTween.Sequence().
            Insert(0, transform.DOLocalRotate(new Vector3(0, 0, _currentAngle), _currentDuration/2, RotateMode.FastBeyond360).SetEase(Ease.Linear)).
            Append(transform.DOLocalRotate(new Vector3(0, 0, 0), _currentDuration/2, _currentAngle >= 270 ? RotateMode.FastBeyond360 : RotateMode.Fast).SetEase(Ease.Linear)).
            AppendCallback(() => transform.rotation = new Quaternion(0, 0, 0, 0)).
            AppendCallback(() => StartRotation());
    }
}
