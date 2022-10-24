using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Animator _animator;
    public float speed = 5f;
    private Rigidbody rb;
    public float jumpSpeed = 150f;
    void Start()
    {
        _animator = GetComponent<Animator>();
        Cursor.lockState = CursorLockMode.Locked;
        rb = gameObject.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
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
        rb.AddForce(Vector3.up * jumpSpeed);
    }
}
