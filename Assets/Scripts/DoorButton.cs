using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorButton : MonoBehaviour
{
    [SerializeField] public int buttonId;
    [SerializeField] public float buttonTimer = 3f;
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
                if (hit.transform.CompareTag("DoorButton"))
                {
                    StartCoroutine(ButtonPress());
                }
            }
        }
    }

    private IEnumerator ButtonPress()
    {
        _sceneManager.DoorAction(buttonId);
        yield return new WaitForSeconds(buttonTimer);
        _sceneManager.DoorAction(buttonId);
    }
}
