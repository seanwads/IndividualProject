using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    [SerializeField] public int plateId;
    private SceneManager _sceneManager;
    void Start()
    {
        _sceneManager = FindObjectOfType<SceneManager>();
    }
    

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Rigidbody>().mass >= 1)
        {
            _sceneManager.DoorAction(plateId);
            Debug.Log("triggered");
        }
    }
}
