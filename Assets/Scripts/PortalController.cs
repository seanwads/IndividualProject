using System;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;
using Vector4 = UnityEngine.Vector4;

public class PortalController : MonoBehaviour
{
    [SerializeField] public int portalId;
    private GameObject _pairedPortal;
    private Transform _targetPos;
    private PlayerController _player;
    private Camera _playerCam;
    private Camera _cam;
    private GameObject _hidableWall;
    private GameObject _parentWall;
    private Collider _wallCollider;

    public float nearClipOffset = 0.05f;
    public float nearClipLimit = 0.2f;

    private ItemPickup _itemToSplit;
    private GameObject _splitItem;
    private Rigidbody _splitItemRb;
    public bool itemInPortal;
    private bool _itemHasPassed;
    private bool _itemIsThrough;

    void Start()
    {
        _player = FindObjectOfType<PlayerController>();
        _playerCam = GameObject.FindGameObjectWithTag("PlayerCamera").GetComponent<Camera>();
        _cam = GetComponentInChildren<Camera>();
        _parentWall = transform.parent.gameObject;
        _wallCollider = _parentWall.GetComponent<Collider>();
        
        _cam.aspect = ((float)Screen.width) / Screen.height;
    }

    private void Update()
    {
        if (gameObject.CompareTag("Portal1"))
        {
            _pairedPortal = GameObject.FindGameObjectWithTag("Portal2");
        }
        else
        {
            _pairedPortal = GameObject.FindGameObjectWithTag("Portal1");
        }

        if (_pairedPortal)
        {
            _targetPos = _pairedPortal.transform.GetChild(0);
        }

    }

    void LateUpdate()
    {
        CameraMovement();
        CalculateClipPlane();

        if (itemInPortal)
        {
            MoveSplitItem();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        _wallCollider.enabled = false;

        if (other.CompareTag("PlayerCenter"))
        {
            _player.Teleport(_targetPos);
            Destroy(_splitItem);
            itemInPortal = false;
            _itemHasPassed = false;
            _itemIsThrough = false;
        }

        if (other.CompareTag("PickupItem"))
        {
            if (!_itemIsThrough)
            {
                _itemToSplit = other.GetComponent<ItemPickup>();
                SplitItem();
            }
            else
            {
                _itemIsThrough = false;
            }
            
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("PickupItem"))
        {
            if (!_itemIsThrough)
            {
                itemInPortal = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        _wallCollider.enabled = true;
        
        if (other.CompareTag("PickupItem"))
        {
            if (_itemHasPassed)
            {
                _itemIsThrough = true;
            }
            else
            {
                itemInPortal = false;
                Destroy(_splitItem);
            }
        }

        if (other.CompareTag("ItemCenter"))
        {
            _itemHasPassed = !_itemHasPassed;
        }
        
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
            _cam.projectionMatrix = _cam.CalculateObliqueMatrix(clipPlaneCameraSpace);
        }
    }

    private void SplitItem()
    {
        _splitItem = Instantiate(_itemToSplit.mirror, _pairedPortal.transform.position, Quaternion.identity);
        _splitItemRb = _splitItem.GetComponent<Rigidbody>();
        
    }

    private void MoveSplitItem()
    {
        _splitItemRb.isKinematic = true;
        _splitItemRb.velocity = Vector3.zero;
        _splitItemRb.angularVelocity = Vector3.zero;
        
        Vector3 itemOffsetPos = transform.InverseTransformPoint(_itemToSplit.transform.position);
        itemOffsetPos = Vector3.Scale(itemOffsetPos, new Vector3(-1, 1, -1));
        _splitItem.transform.position = _pairedPortal.transform.TransformPoint(itemOffsetPos);
        
        Vector3 itemOffsetRot = transform.InverseTransformDirection(_itemToSplit.transform.forward);
        itemOffsetRot = Vector3.Scale(itemOffsetRot, new Vector3(-1, 1, -1));
        _splitItem.transform.forward = _pairedPortal.transform.TransformDirection(itemOffsetRot);
    }

}