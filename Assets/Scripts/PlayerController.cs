using UnityEngine;


public class PlayerController : MonoBehaviour
{
    [SerializeField] public GameObject portal1;
    [SerializeField] public GameObject portal2;
    private PortalController _curPortal1;
    private PortalController _curPortal2;
    
    private Animator _animator;
    public float speed = 5f;
    private Rigidbody _rb;
    public float jumpSpeed = 150f;
    private Vector3 _velocity;
    private CameraController _cameraController;
    private Transform _pickupAnchor;

    private Camera _camera;
    private bool _isHoldingItem;
    public float throwingForce = 4f;

    public float portalShootDistance = 30f;
    private LayerMask _ignoreRaycast;
    
    [SerializeField] public float minPortalHeight;
    [SerializeField] public float maxPortalHeight;
    void Start()
    {
        _curPortal1 = GameObject.FindGameObjectWithTag("Portal1").GetComponent<PortalController>();
        _curPortal2 = GameObject.FindGameObjectWithTag("Portal2").GetComponent<PortalController>();
        
        _animator = GetComponent<Animator>();
        Cursor.lockState = CursorLockMode.Locked;
        _rb = gameObject.GetComponent<Rigidbody>();
        _cameraController = GetComponentInChildren<CameraController>();
        _pickupAnchor = GameObject.FindGameObjectWithTag("PickupAnchor").transform;
        _camera = GetComponentInChildren<Camera>();
        _ignoreRaycast = LayerMask.GetMask("Ignore Raycast");
    }

    // Update is called once per frame
    void Update()
    {
        _velocity = _rb.velocity;
        
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        _animator.SetFloat("Speed", v);
        _animator.SetFloat("Rotation", h);
        
        transform.Translate(h * speed * Time.deltaTime, 0, v * speed * Time.deltaTime);

        if (Input.GetKeyDown("escape"))
        {
            Cursor.lockState = CursorLockMode.None;
        }

        if (Input.GetKeyDown("space"))
        {
            Jump();
        }

        if (Input.GetKeyDown("f"))
        {
            if (_isHoldingItem)
            {
                if (!_curPortal1.itemInPortal && !_curPortal2.itemInPortal)
                {
                    DropItem();
                }
            }
            else
            {
                Ray ray = _camera.ViewportPointToRay(Vector3.one * 0.5f);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, 5f))
                {
                    ItemPickup item = hit.transform.GetComponent<ItemPickup>();
                    if (item)
                    {
                        PickupItem(item);
                    }
                }
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            ShootPortal(1);
        }

        if (Input.GetMouseButtonDown(1))
        {
            ShootPortal(2);
        }
    }

    private void Jump()
    {
        _animator.SetTrigger("Jump");
        _rb.AddForce(Vector3.up * jumpSpeed);
    }

    public void Teleport(Transform target)
    {
        transform.position = target.position;
        _cameraController.mouseLook = new Vector2(target.eulerAngles.y, 0f);
        _rb.AddForce(_velocity);
    }
    
    private void DropItem()
    {
        ItemPickup i = _pickupAnchor.GetChild(0).GetComponent<ItemPickup>();

        i.transform.parent = null;
        Rigidbody rb = i.GetComponent<Rigidbody>();
        rb.isKinematic = false;
        rb.AddForce(transform.forward * throwingForce, ForceMode.VelocityChange);

        _isHoldingItem = false;
    }

    private void PickupItem(ItemPickup i)
    {
        _isHoldingItem = true;
        
        Rigidbody rb = i.GetComponent<Rigidbody>();
        rb.isKinematic = true;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        
        i.transform.SetParent(_pickupAnchor);
        i.transform.localPosition = Vector3.zero;
    }

    private void ShootPortal(int portalNum)
    {
        Ray ray = _camera.ViewportPointToRay(Vector3.one * 0.5f);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, portalShootDistance, ~_ignoreRaycast))
        {
            if (hit.transform.CompareTag("PortalWall"))
            {
                
                if (hit.point.y < minPortalHeight)
                {
                    hit.point = new Vector3(hit.point.x, minPortalHeight, hit.point.z);
                }
                else if (hit.point.y > maxPortalHeight)
                {
                    hit.point = new Vector3(hit.point.x, maxPortalHeight, hit.point.z);
                }
                
                
                if (portalNum == 1)
                {
                    if (_curPortal1 != null)
                    {
                        Destroy(_curPortal1.gameObject);
                    }
                    _curPortal1 = Instantiate(portal1, hit.point, hit.transform.rotation).GetComponent<PortalController>();
                    _curPortal1.transform.SetParent(hit.transform, true);

                }
                else if (portalNum == 2)
                {
                    if (_curPortal2 != null)
                    {
                        Destroy(_curPortal2.gameObject);
                    }
                    _curPortal2 = Instantiate(portal2, hit.point, hit.transform.rotation).GetComponent<PortalController>();
                    _curPortal2.transform.SetParent(hit.transform, true);
                }
                
            }
        }
    }
}
