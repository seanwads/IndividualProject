using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using UnityEngine;

public class SceneManager : MonoBehaviour
{
    [SerializeField] public GameObject[] rooms;
    public Dictionary<int, DoorController> _doors = new Dictionary<int, DoorController>();

    void Start()
    {
        DoorController[] tempDoors = FindObjectsOfType<DoorController>();
        foreach (DoorController d in tempDoors)
        {
            _doors.Add(d.doorId, d);
        }
    }

    public void DoorAction(int id)
    {
        DoorController door = _doors[id];
        if (door.transform.GetChild(0).gameObject.activeSelf)
        {
            door.OpenDoor();
        }
        else
        {
            door.CloseDoor();
        }
    }

    public void NextRoom(int roomNum)
    {
        rooms[roomNum].SetActive(false);
        rooms[roomNum].SetActive(true);
    }
}
