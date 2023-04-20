using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    private SceneManager _sceneManager;

    void Start()
    {
        _sceneManager = FindObjectOfType<SceneManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _sceneManager.curCheckpoint = this.transform;
        }
    }
}
