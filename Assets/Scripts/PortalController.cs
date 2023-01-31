using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class PortalController : MonoBehaviour
{
    private GameObject _pairedPortal;
    private Transform _targetPos;
    private PlayerController _player;
    private GameObject _playerCam;
    private Camera _cam;
    private GameObject _wall;
    private Collider _wallCollider;
    

    void Start()
    {
        _player = FindObjectOfType<PlayerController>();
        _playerCam = GameObject.FindGameObjectWithTag("PlayerCamera");
        _cam = GetComponentInChildren<Camera>();
        _wall = transform.parent.gameObject;
        _wallCollider = _wall.GetComponent<Collider>();
        
        if (gameObject.CompareTag("Portal1")) 
        {
            _pairedPortal = GameObject.FindGameObjectWithTag("Portal2");
        }
        else
        {
            _pairedPortal = GameObject.FindGameObjectWithTag("Portal1");
        }

        int wallLayer = LayerMask.NameToLayer("PortalWalls");
        _wall.layer = wallLayer;
    }
    
    void Update()
    {
        CameraMovement();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _wallCollider.enabled = false;
        }

        if (other.CompareTag("PlayerCenter"))
        {
            _targetPos = _pairedPortal.transform.GetChild(0);
            _player.Teleport(_targetPos);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        _wallCollider.enabled = true;
    }

    private void CameraMovement()
    {
        Vector3 playerPos = _playerCam.transform.position - _pairedPortal.transform.position;

        _cam.transform.localRotation = Quaternion.LookRotation(playerPos, Vector3.up);
        _cam.transform.position = new Vector3(transform.position.x - playerPos.x, _playerCam.transform.position.y,
            transform.position.z - playerPos.z);
    }
}