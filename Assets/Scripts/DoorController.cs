using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    [SerializeField] public int doorId;
    private GameObject _doorMesh;

    void Start()
    {
        _doorMesh = transform.GetChild(0).gameObject;
    }

    public void OpenDoor()
    {
        _doorMesh.SetActive(false);
    }

    public void CloseDoor()
    {
        _doorMesh.SetActive(true);
    }
}
