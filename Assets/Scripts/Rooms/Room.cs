using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    [SerializeField] private List<PortalDoor> portalDoors;

    [SerializeField] private List<Room> connectedRooms;

    public void EnterRoom()
    {
        PortalManager.instance.AssignPortalDoors(portalDoors);
    }
}
