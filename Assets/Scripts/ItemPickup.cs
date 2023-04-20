using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    [SerializeField] public GameObject mirror;
    private Vector3 _startPosition;
    void Start()
    {
        _startPosition = transform.position;
    }

    public void Reset()
    {
        transform.position = _startPosition;
    }
}
