using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Animator _animator;
    public Camera camera;
    public float camSpeed;
    private float _yRotation;
    private float _xRotation;
    void Start()
    {
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        float x = Input.GetAxis("Mouse X");
        float y = Input.GetAxis("Mouse Y");
        
        _yRotation += y * camSpeed;
        _xRotation += x * camSpeed;
        _yRotation = Mathf.Clamp(_yRotation, -90, 90);
        
        var xQuat = Quaternion.AngleAxis(_xRotation, Vector3.up);
        var yQuat = Quaternion.AngleAxis(_yRotation, Vector3.left);

        camera.transform.localRotation = xQuat * yQuat;
        
        _animator.SetFloat("Speed", v);
        _animator.SetFloat("Rotation", h);
        
    }
}
