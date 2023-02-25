using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    [SerializeField] public int doorId;

    public void OpenDoor()
    {
        gameObject.SetActive(false);
    }

    public void CloseDoor()
    {
        gameObject.SetActive(true);
    }
}
