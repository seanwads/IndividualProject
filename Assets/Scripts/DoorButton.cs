using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorButton : MonoBehaviour
{
    [SerializeField] public int buttonId;
    private Camera _cam;
    private SceneManager _sceneManager;
    
    void Start()
    {
        _cam = GameObject.FindGameObjectWithTag("PlayerCamera").GetComponent<Camera>();
        _sceneManager = FindObjectOfType<SceneManager>();
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
                    _sceneManager.DoorAction(buttonId);
                    Debug.Log("Button triggered");
                }
            }
        }
    }
}
