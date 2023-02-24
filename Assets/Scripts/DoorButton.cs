using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorButton : MonoBehaviour
{
    private Camera _cam;
    void Start()
    {
        _cam = GameObject.FindGameObjectWithTag("PlayerCamera").GetComponent<Camera>();
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Ray ray = _cam.ViewportPointToRay(Vector3.one * 0.5f);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 3f))
            {
                if (hit.transform.GetComponent<DoorButton>() != null)
                {
                    //button triggered
                    Debug.Log("Button triggered");
                }
            }
        }
    }
}
