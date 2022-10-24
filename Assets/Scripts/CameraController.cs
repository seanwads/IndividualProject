using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float sensitivity;
    public float smoothing;
    private GameObject _player;
    private Vector2 _mouseLook;
    private Vector2 _smoothV;
    void Start()
    {
        sensitivity = 1f;
        smoothing = 2f;
        _player = transform.parent.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        var md = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));
        md = Vector2.Scale(md, new Vector2(sensitivity * smoothing, sensitivity * smoothing));
        
        // the interpolated float result between the two float values
        _smoothV.x = Mathf.Lerp(_smoothV.x, md.x, 1f / smoothing);
        _smoothV.y = Mathf.Lerp(_smoothV.y, md.y, 1f / smoothing);
        // incrementally add to the camera look
        _mouseLook += _smoothV;

        _mouseLook.y = Mathf.Clamp(_mouseLook.y,-90f, 90f);
        
        // vector3.right means the x-axis
        transform.localRotation = Quaternion.AngleAxis(-_mouseLook.y, Vector3.right);
       _player.transform.localRotation = Quaternion.AngleAxis(_mouseLook.x, _player.transform.up);
    }
}
