using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    [SerializeField] public int plateId;
    [SerializeField] public float massRequirement = 1f;
    private SceneManager _sceneManager;
    private bool _plateActive;
    
    void Start()
    {
        _sceneManager = FindObjectOfType<SceneManager>();
    }
    

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Rigidbody>().mass >= massRequirement && !_plateActive)
        {
            _sceneManager.DoorAction(plateId);
            _plateActive = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<Rigidbody>().mass >= massRequirement)
        {
            _sceneManager.DoorAction(plateId);
            _plateActive = false;
        }
    }
}
