using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillPlane : MonoBehaviour
{
    private PlayerController _player;
    void Start()
    {
        _player = FindObjectOfType<PlayerController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _player.TakeDamage(100f);
        }
    }
}
