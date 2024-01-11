using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using TMPro;

public class GameManager : MonoBehaviour
{
    //--------------
    // Dependencies
    //--------------
    [SerializeField] Transform rooms;
    [SerializeField] int currentRoom;
    [SerializeField] Transform player;
    [SerializeField] CinemachineVirtualCamera playerVirCam;
    [SerializeField] InputManager inputManager;
    [SerializeField] TMP_Text roomNumber_txt;

    string gamestate;

    private void Start()
    {
        var virCam = rooms.GetChild(currentRoom).Find("VirtualCamera");
        virCam.GetComponent<CinemachineVirtualCamera>().Priority++;
        DisplayUpdate();
    }

    void KeyTaken()
    {
        GetCurrentRoomEditor().GetDoor().StartAnimator();
    }

    void GameStates()
    {
        switch (gamestate)
        {
            case "Menu":
                if (inputManager.GetEsc() > 0)
                {
                    Application.Quit();
                }
                break;
            case "InGame":
                if (inputManager.GetEsc() > 0)
                {
                    BackToMenu();
                }
                break;

        }
    }
    private void Update()
    {
        GameStates();
    }

    void DisplayUpdate()
    {
        roomNumber_txt.text = currentRoom + 1.ToString() + " / " + rooms.childCount.ToString();
    }
    //======================
    //    PUBLIC METHODS
    //======================
    public void StartGame()
    {
        GetCurrentRoom().GetComponent<PivotRotateCeiling>().StartRotate();
        player.gameObject.SetActive(true);
        player.position = GetCurrentRoom().Find("PlayerSpawnPosition").position;
        playerVirCam.Priority = 1;
        gamestate = "InGame";
    }
    public void BackToMenu()
    {
        currentRoom = 0;
        var virCam = GetCurrentRoom().Find("VirtualCamera");
        virCam.GetComponent<CinemachineVirtualCamera>().Priority = 1;
        player.gameObject.SetActive(false);
        playerVirCam.Priority = -1;
        gamestate = "Menu";
        DisplayUpdate();
    }
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
    public void NextRoom()
    {
        var oldvirCam = rooms.GetChild(currentRoom).Find("VirtualCamera");
        currentRoom++;
        currentRoom = Mathf.Clamp(currentRoom, 0, rooms.childCount-1);
        var virCam = rooms.GetChild(currentRoom).Find("VirtualCamera");
        oldvirCam.GetComponent<CinemachineVirtualCamera>().Priority--;
        virCam.GetComponent<CinemachineVirtualCamera>().Priority++;
        DisplayUpdate();
    }
    public void PreviousRoom()
    {
        var oldvirCam = rooms.GetChild(currentRoom).Find("VirtualCamera");
        currentRoom--;
        currentRoom = Mathf.Clamp(currentRoom, 0, rooms.childCount-1);
        var virCam = rooms.GetChild(currentRoom).Find("VirtualCamera");
        oldvirCam.GetComponent<CinemachineVirtualCamera>().Priority--;
        virCam.GetComponent<CinemachineVirtualCamera>().Priority++;
        DisplayUpdate();
    }
}
