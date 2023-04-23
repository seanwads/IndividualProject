using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Rendering;

public class SceneManager : MonoBehaviour
{
    public Dictionary<int, DoorController> _doors = new Dictionary<int, DoorController>();
    private PlayerController _player;
    public Transform curCheckpoint;
    public int checkpointNum;
    private ItemPickup[] _items;
    
    void Start()
    {
        DoorController[] tempDoors = FindObjectsOfType<DoorController>();
        foreach (DoorController d in tempDoors)
        {
            _doors.Add(d.doorId, d);
        }
        _player = FindObjectOfType<PlayerController>();
        _items = FindObjectsOfType<ItemPickup>();
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

    public void Respawn()
    {
        foreach (ItemPickup i in _items)
        {
            i.Reset();
        }

        _player.transform.position = curCheckpoint.transform.position;
        _player.currentHealth = 100f;
        _player.enabled = true;
    }
}
