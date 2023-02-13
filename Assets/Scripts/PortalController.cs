using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Rendering;
using Matrix4x4 = UnityEngine.Matrix4x4;
using Plane = UnityEngine.Plane;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;
using Vector4 = UnityEngine.Vector4;

public class PortalController : MonoBehaviour
{
    public GameObject _pairedPortal;
    public Transform _targetPos;
    private PlayerController _player;
    private Camera _playerCam;
    private Camera _cam;
    private GameObject _hidableWall;
    private GameObject _parentWall;
    private Collider _wallCollider;

    public float nearClipOffset = 0.05f;
    public float nearClipLimit = 0.2f;

    void Start()
    {
        _player = FindObjectOfType<PlayerController>();
        _playerCam = GameObject.FindGameObjectWithTag("PlayerCamera").GetComponent<Camera>();
        _cam = GetComponentInChildren<Camera>();
        _parentWall = transform.parent.gameObject;
        _wallCollider = _parentWall.GetComponent<Collider>();

        if (gameObject.CompareTag("Portal1")) 
        {
            _pairedPortal = GameObject.FindGameObjectWithTag("Portal2");
            _pairedPortal.transform.GetChild(2).gameObject.layer = LayerMask.NameToLayer("Portal1Cull");

            
        }
        else
        {
            _pairedPortal = GameObject.FindGameObjectWithTag("Portal1");
            _pairedPortal.transform.GetChild(2).gameObject.layer = LayerMask.NameToLayer("Portal2Cull");
        }
        
        _targetPos = _pairedPortal.transform.GetChild(0);
    }
    
    void LateUpdate()
    {
        CameraMovement();
        CalculateClipPlane();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _wallCollider.enabled = false;
        }

        if (other.CompareTag("PlayerCenter"))
        {
            _player.Teleport(_targetPos);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        _wallCollider.enabled = true;
    }

    private void CameraMovement()
    {
        Vector3 offsetPos = _pairedPortal.transform.InverseTransformPoint(_playerCam.transform.position);
        offsetPos = Vector3.Scale(offsetPos, new Vector3(-1, 1, -1));
        _cam.transform.position = transform.TransformPoint(offsetPos);

        Vector3 offsetRot = _pairedPortal.transform.InverseTransformDirection(_playerCam.transform.forward);
        offsetRot = Vector3.Scale(offsetRot, new Vector3(-1, 1, -1));
        _cam.transform.forward = transform.TransformDirection(offsetRot);
    }

    private void CalculateClipPlane()
    {
        Transform clipPlane = transform;
        int dot = System.Math.Sign (Vector3.Dot (clipPlane.forward, transform.position - _cam.transform.position));

        Vector3 camSpacePos = _cam.worldToCameraMatrix.MultiplyPoint (clipPlane.position);
        Vector3 camSpaceNormal = _cam.worldToCameraMatrix.MultiplyVector (clipPlane.forward) * dot;
        float camSpaceDst = -Vector3.Dot (camSpacePos, camSpaceNormal) + nearClipOffset;

        // Don't use oblique clip plane if very close to portal as it seems this can cause some visual artifacts
        if (Mathf.Abs (camSpaceDst) > nearClipLimit) {
            Vector4 clipPlaneCameraSpace = new Vector4 (camSpaceNormal.x, camSpaceNormal.y, camSpaceNormal.z, camSpaceDst);

            // Update projection based on new clip plane
            // Calculate matrix with player cam so that player camera settings (fov, etc) are used
            _cam.projectionMatrix = _playerCam.CalculateObliqueMatrix (clipPlaneCameraSpace);
        }
    }
}