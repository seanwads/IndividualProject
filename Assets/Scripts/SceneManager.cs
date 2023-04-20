using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using UnityEngine;

public class SceneManager : MonoBehaviour
{
    [SerializeField] public GameObject[] rooms;
    public Dictionary<int, DoorController> _doors = new Dictionary<int, DoorController>();
    private GameObject[] _spawnPoints;

    void Start()
    {
        DoorController[] tempDoors = FindObjectsOfType<DoorController>();
        foreach (DoorController d in tempDoors)
        {
            _doors.Add(d.doorId, d);
        }

        _spawnPoints = GameObject.FindGameObjectsWithTag("SpawnPoint");
    }

    public void DoorAction(int id)
    {
        DoorController door = _doors[id];
        
        if (door.doorOpen)
        {
            door.CloseDoor();
        }
        else
        {
            door.OpenDoor();
        }
    }
}
