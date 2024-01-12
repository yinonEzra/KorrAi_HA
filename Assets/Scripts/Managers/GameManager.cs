using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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
    [SerializeField] GameObject MenuCanvas;
    [SerializeField] GameObject InGameCanvas;
    [SerializeField] GameObject WinMenuCanvas;
    [SerializeField] GameObject LoseMenuCanvas;
    [SerializeField] GameObject PauseMenuCanvas;

    string gamestate;
    private void Awake()
    {
        InitMenuState();
        Screen.SetResolution(1920, 1080, true);
    }
    private void InitMenuState()
    {
        var virCam = rooms.GetChild(currentRoom).Find("VirtualCamera");
        virCam.GetComponent<CinemachineVirtualCamera>().Priority = 1;
        gamestate = "Menu";
        Color greyd = Color.white;
        greyd.a = 0.1f;
        InGameCanvas.transform.Find("Key (img)").GetComponent<Image>().color = greyd;
        DisplayUpdate();
    }

    void KeyTaken()
    {
        GetCurrentRoomEditor().GetDoor().OpenDoorAnim();
        InGameCanvas.transform.Find("Key (img)").GetComponent<Image>().color = Color.white;
    }

    void GameStates()
    {
        switch (gamestate)
        {
            case "Menu":
                MenuCanvas.SetActive(true);
                WinMenuCanvas.SetActive(false);
                LoseMenuCanvas.SetActive(false);
                InGameCanvas.SetActive(false);
                PauseMenuCanvas.SetActive(false);
                Cursor.visible = true;
                inputManager.SetFreeze(true);
                if (inputManager.GetEsc() > 0)
                {
                    QuitGame();
                }
                break;
            case "InGame":
                Cursor.visible = false;
                WinMenuCanvas.SetActive(false);
                LoseMenuCanvas.SetActive(false);
                inputManager.SetFreeze(false);
                MenuCanvas.SetActive(false);
                PauseMenuCanvas.SetActive(false);
                InGameCanvas.SetActive(true);
                if (inputManager.GetEsc() > 0)
                {
                    gamestate = "PauseGame";
                }
                break;
            case "WinGame":
                playerVirCam.Priority = 0;
                WinMenuCanvas.SetActive(true);
                MenuCanvas.SetActive(false);
                LoseMenuCanvas.SetActive(false);
                InGameCanvas.SetActive(false);
                PauseMenuCanvas.SetActive(false);
                Cursor.visible = true;
                inputManager.SetFreeze(true);
                player.gameObject.SetActive(false);
                if (inputManager.GetEsc() > 0)
                {
                    FromWinBackToMenu();
                }
                break;
            case "LoseGame":
                playerVirCam.Priority = 0;
                LoseMenuCanvas.SetActive(true);
                WinMenuCanvas.SetActive(false);
                MenuCanvas.SetActive(false);
                InGameCanvas.SetActive(false);
                PauseMenuCanvas.SetActive(false);
                Cursor.visible = true;
                inputManager.SetFreeze(true);
                player.gameObject.SetActive(false);
                if (inputManager.GetEsc() > 0)
                {
                    FromLoseBackToMenu();
                }
                break;
            case "PauseGame":
                PauseMenuCanvas.SetActive(true);
                LoseMenuCanvas.SetActive(false);
                WinMenuCanvas.SetActive(false);
                MenuCanvas.SetActive(false);
                InGameCanvas.SetActive(false);
                Cursor.visible = true;
                inputManager.SetFreeze(true);
                if (inputManager.GetEsc() > 0)
                {
                    gamestate = "InGame";
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
        var disCurrentRoom = currentRoom + 1;
        roomNumber_txt.text = disCurrentRoom.ToString() + " / " + rooms.childCount.ToString();
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
    public void Gameover()
    {
        gamestate = "LoseGame";
        GetCurrentRoomEditor().GetDoor().GetDoorVCam().Priority = 1;
    }
    public void GameWin()
    {
        gamestate = "WinGame";
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
    public void FromWinBackToMenu()
    {
        GetCurrentRoomEditor().GetDoor().GetDoorVCam().Priority = 0;
        InitMenuState();
    }
    public void FromLoseBackToMenu()
    {
        GetCurrentRoomEditor().GetDoor().GetDoorVCam().Priority = 0;
        InitMenuState();
    }
    public void ResumeGame()
    {
        gamestate = "InGame";
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
