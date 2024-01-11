using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //--------------
    // Dependencies
    //--------------
    [SerializeField] Transform rooms;
    [SerializeField] int currentRoom;
    


    void KeyTaken()
    {
        GetCurrentRoomEditor().GetDoor().StartAnimator();
    }



    //======================
    //    PUBLIC METHODS
    //======================
    public Transform GetCurrentRoom()
    {
        return rooms.GetChild(currentRoom);
    }
    public RoomEditor GetCurrentRoomEditor()
    {
        var roomTransform = rooms.GetChild(currentRoom);
        return roomTransform.gameObject.GetComponent<RoomEditor>();
    }
    public void CallKeyTaken()
    {
        KeyTaken();
    }
}
