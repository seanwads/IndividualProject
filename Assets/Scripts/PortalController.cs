using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalController : MonoBehaviour
{
    private GameObject _pairedPortal;
    private Transform _targetPos;

    private GameObject _player;
    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        
        if (gameObject.CompareTag("Portal1")) 
        {
            _pairedPortal = GameObject.FindGameObjectWithTag("Portal2");
        }
        else
        {
            _pairedPortal = GameObject.FindGameObjectWithTag("Portal1");
        }
    }
    
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _targetPos = _pairedPortal.transform.GetChild(0);
            other.transform.position = _targetPos.position;
        }
    }
}