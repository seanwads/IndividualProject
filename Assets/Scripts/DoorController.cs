using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    [SerializeField] public int doorId;
    private Animator _animator;
    public bool doorOpen;

    void Start()
    {
        _animator = GetComponent<Animator>();
    }

    public void OpenDoor()
    {
        _animator.Play("DoorOpen");
        doorOpen = true;
    }

    public void CloseDoor()
    {
        _animator.Play("DoorClose");
        doorOpen = false;
    }
    
}
