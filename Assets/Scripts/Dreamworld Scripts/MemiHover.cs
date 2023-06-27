using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MemiHover : MonoBehaviour
{
    private Vector3 _startPosition;
    public Transform playerPosition;
    
    [SerializeField] private float _modifier;
    [SerializeField] private float _speedModifier;

    public bool isHover = false;
    public void OnHover()
    {
        Vector3 playerPos = playerPosition.position;
        transform.position = new Vector3(playerPos.x+ 3.5f, playerPos.y + 2.5f, 0);
        _startPosition = transform.position;
        isHover = true;
    }

    public void DisableHover()
    {
        isHover = false;
    }


    private void FixedUpdate()
    {
   
if(isHover){
            //transform.position = _startPos + Vector3.up * Mathf.Sin(Time.time * _spdMod) * _mod;
            float time = Time.time;
            transform.position = _startPosition + Vector3.up * Mathf.Sin(time * _speedModifier) * _modifier;
}
    }
}
