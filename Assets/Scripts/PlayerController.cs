using UnityEngine;


public class PlayerController : MonoBehaviour
{
    private PortalController _portal1;
    private PortalController _portal2;
    
    private Animator _animator;
    public float speed = 5f;
    private Rigidbody _rb;
    public float jumpSpeed = 150f;
    private Vector3 _velocity;
    private CameraController _cameraController;
    private Transform _pickupAnchor;

    private Camera _camera;
    private bool _isHoldingItem;
    public bool canDropItem;
    public float throwingForce = 4f;
    void Start()
    {
        _portal1 = GameObject.FindGameObjectWithTag("Portal1").GetComponent<PortalController>();
        _portal2 = GameObject.FindGameObjectWithTag("Portal2").GetComponent<PortalController>();
        
        _animator = GetComponent<Animator>();
        Cursor.lockState = CursorLockMode.Locked;
        _rb = gameObject.GetComponent<Rigidbody>();
        _cameraController = GetComponentInChildren<CameraController>();
        _pickupAnchor = GameObject.FindGameObjectWithTag("PickupAnchor").transform;
        _camera = GetComponentInChildren<Camera>();
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

        if (Input.GetMouseButtonDown(1))
        {
            if (_isHoldingItem)
            {
                if (!_portal1.itemInPortal && !_portal2.itemInPortal)
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
}
