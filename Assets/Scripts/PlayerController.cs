using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Animator _animator;
    public float speed = 5f;
    private Rigidbody _rb;
    public float jumpSpeed = 150f;
    private Vector3 _velocity;
    private CameraController _cameraController;
    void Start()
    {
        _animator = GetComponent<Animator>();
        Cursor.lockState = CursorLockMode.Locked;
        _rb = gameObject.GetComponent<Rigidbody>();
        _cameraController = GetComponentInChildren<CameraController>();
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
    }

    private void Jump()
    {
        _animator.SetTrigger("Jump");
        _rb.AddForce(Vector3.up * jumpSpeed);
    }

    public void Teleport(Transform target)
    {
        Vector3 lookDirection = target.position - transform.position;
        
        transform.position = target.position;
        _cameraController.mouseLook = new Vector2(0f, 0f);
        _rb.AddForce(_velocity);
    }
}
