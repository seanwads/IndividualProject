using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using UnityEditor;
using UnityEngine;


public class PlayerController : MonoBehaviour
{
    private GameMenuManager _gameMenuManager;
    private SceneManager _sceneManager;

    [SerializeField] private GameObject portal1;
    [SerializeField] private GameObject portal2;
    private PortalController _curPortal1;
    private PortalController _curPortal2;

    private Animator _animator;
    [SerializeField] private float speed = 5f;
    private Rigidbody _rb;
    [SerializeField] private float jumpSpeed = 150f;
    private Vector3 _velocity;
    private CameraController _cameraController;
    private Transform _pickupAnchor;

    private Camera _camera;
    private bool _isHoldingItem;
    [SerializeField] private float throwingForce = 4f;

    public float portalShootDistance = 30f;
    private LayerMask _ignoreRaycast;

    [SerializeField] private float minPortalHeight;
    [SerializeField] private float maxPortalHeight;

    private float _maxHealth = 100f;
    public float currentHealth = 100f;

    private float _fallDamageTimer;
    private bool _isGrounded = true;
    private bool _isFalling;
    [SerializeField] private float fallDamageThreshold;
    [SerializeField] private float fallDamageMultiplier;

    private int _checkpointNum;

    void Start()
    {
        _gameMenuManager = FindObjectOfType<GameMenuManager>();
        _sceneManager = FindObjectOfType<SceneManager>();
        _animator = GetComponent<Animator>();
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

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            speed = 7f;
        }

        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            speed = 5f;
        }

        if (Input.GetKeyDown("e"))
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

        _checkpointNum = _sceneManager.checkpointNum;

    }

    private void Jump()
    {
        _animator.SetTrigger("Jump");
        _rb.AddForce(Vector3.up * jumpSpeed);
    }

    private void CheckFallDamage()
    {
        RaycastHit hit;
        _isGrounded = Physics.Raycast(transform.position, -Vector3.up, out hit, 0.1f);
        if (!_isGrounded)
        {
            _fallDamageTimer += Time.deltaTime;
            _isFalling = true;
        }
        else
        {
            if (_isFalling)
            {
                if (_fallDamageTimer > fallDamageThreshold)
                {
                    TakeDamage(_fallDamageTimer * fallDamageMultiplier);
                }

                _fallDamageTimer = 0f;
                _isFalling = false;
            }
        }
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

                CheckMinMaxHeight();
                
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

                    _curPortal1 = Instantiate(portal1, hit.point, hit.transform.rotation)
                        .GetComponent<PortalController>();
                    _curPortal1.transform.SetParent(hit.transform, true);

                }
                else if (portalNum == 2)
                {
                    if (_curPortal2 != null)
                    {
                        Destroy(_curPortal2.gameObject);
                    }

                    _curPortal2 = Instantiate(portal2, hit.point, hit.transform.rotation)
                        .GetComponent<PortalController>();
                    _curPortal2.transform.SetParent(hit.transform, true);
                }

            }
        }
    }

    private void CheckMinMaxHeight()
    {
        switch (_checkpointNum)
        {
            case 1:
                minPortalHeight = -2.2f;
                maxPortalHeight = -0.2f;
                break;
            case 2:
                minPortalHeight = 2.3f;
                maxPortalHeight = 9.3f;
                break;
            case 3:
                minPortalHeight = -2.75f;
                maxPortalHeight = 4.6f;
                break;
            case 4:
                minPortalHeight = -2.75f;
                maxPortalHeight = 4.6f;
                break;
        }
    }

    public float GetHealthPercent()
    {
        float hp = currentHealth / _maxHealth;
        return hp;
    }

    public void TakeDamage(float damage)
    {
        currentHealth = currentHealth - damage;
        
        if (currentHealth <= 0)
        {
            _gameMenuManager.PlayerDeath();
            this.enabled = false;
        }
    }
}
