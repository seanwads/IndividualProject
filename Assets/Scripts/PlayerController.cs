using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Animator _animator;
    public float speed;
    void Start()
    {
        _animator = GetComponent<Animator>();
        speed = 1f;
        Cursor.lockState = CursorLockMode.Locked;
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
        
    }
}
