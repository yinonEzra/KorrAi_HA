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

    bool isKeyTaken;
    GameStates currentState;

    public enum GameStates
    {
        Menu = 0,
        InGame = 1,
        WinGame = 2,
        LoseGame = 3,
        PauseGame = 4
    }

    private void Awake()
    {
        InitMenuState();
        Screen.SetResolution(1920, 1080, true);
    }
    private void Update()
    {
        if (currentState == GameStates.InGame)
        {
            if(inputManager.GetEsc() > 0)
            {
                CurrentGameState(GameStates.PauseGame);
            }
        }
    }
    void CurrentGameState(GameStates chosenState)
    {
        switch (chosenState)
        {
            case GameStates.Menu:
                currentState = GameStates.Menu;
                MenuCanvas.SetActive(true);
                WinMenuCanvas.SetActive(false);
                LoseMenuCanvas.SetActive(false);
                InGameCanvas.SetActive(false);
                PauseMenuCanvas.SetActive(false);
                Cursor.visible = true;
                inputManager.SetFreeze(true);
                break;

            case GameStates.InGame:
                currentState = GameStates.InGame;
                Cursor.visible = false;
                WinMenuCanvas.SetActive(false);
                LoseMenuCanvas.SetActive(false);
                inputManager.SetFreeze(false);
                MenuCanvas.SetActive(false);
                PauseMenuCanvas.SetActive(false);
                InGameCanvas.SetActive(true);
                break;

            case GameStates.WinGame:
                currentState = GameStates.WinGame;
                playerVirCam.Priority = 0;
                WinMenuCanvas.SetActive(true);
                MenuCanvas.SetActive(false);
                LoseMenuCanvas.SetActive(false);
                InGameCanvas.SetActive(false);
                PauseMenuCanvas.SetActive(false);
                Cursor.visible = true;
                inputManager.SetFreeze(true);
                player.gameObject.SetActive(false);
                break;

            case GameStates.LoseGame:
                currentState = GameStates.LoseGame;
                playerVirCam.Priority = 0;
                LoseMenuCanvas.SetActive(true);
                WinMenuCanvas.SetActive(false);
                MenuCanvas.SetActive(false);
                InGameCanvas.SetActive(false);
                PauseMenuCanvas.SetActive(false);
                Cursor.visible = true;
                inputManager.SetFreeze(true);
                player.gameObject.SetActive(false);
                break;

            case GameStates.PauseGame:
                currentState = GameStates.PauseGame;
                PauseMenuCanvas.SetActive(true);
                LoseMenuCanvas.SetActive(false);
                WinMenuCanvas.SetActive(false);
                MenuCanvas.SetActive(false);
                InGameCanvas.SetActive(false);
                Cursor.visible = true;
                inputManager.SetFreeze(true);
                break;
        }
    }
    private void InitMenuState()
    {
        var virCam = rooms.GetChild(currentRoom).Find("VirtualCamera");
        virCam.GetComponent<CinemachineVirtualCamera>().Priority = 1;
        CurrentGameState(GameStates.Menu);
        ResetPlayerValues();
        updateRoomCount();
    }

    void KeyTaken()
    {
        GetCurrentRoomEditor().GetDoor().OpenDoorAnim();
        InGameCanvas.transform.Find("Key (img)").GetComponent<Image>().color = Color.white;
        isKeyTaken = true;
    }

    void updateRoomCount()
    {
        var disCurrentRoom = currentRoom + 1;
        roomNumber_txt.text = disCurrentRoom.ToString() + " / " + rooms.childCount.ToString();
    }
    void ResetPlayerValues()
    {
        Color greyd = Color.white;
        greyd.a = 0.1f;
        InGameCanvas.transform.Find("Key (img)").GetComponent<Image>().color = greyd;
        isKeyTaken = false;
        player.GetComponent<PlayerStats>().ResetStats();
    }
    //======================
    //    PUBLIC METHODS
    //======================
    public void QuitGame()
    {
        Application.Quit();
    }

    //-------------------
    //     Menu State 
    //-------------------
    public void StartGame()
    {
        GetCurrentRoom().GetComponent<PivotRotateCeiling>().StartRotate();
        player.gameObject.SetActive(true);
        player.position = GetCurrentRoom().Find("PlayerSpawnPosition").position;
        playerVirCam.Priority = 1;
        CurrentGameState(GameStates.InGame);
    }
    public void NextRoom()
    {
        var oldvirCam = rooms.GetChild(currentRoom).Find("VirtualCamera");
        currentRoom++;
        currentRoom = Mathf.Clamp(currentRoom, 0, rooms.childCount-1);
        var virCam = rooms.GetChild(currentRoom).Find("VirtualCamera");
        oldvirCam.GetComponent<CinemachineVirtualCamera>().Priority--;
        virCam.GetComponent<CinemachineVirtualCamera>().Priority++;
        updateRoomCount();
    }
    public void PreviousRoom()
    {
        var oldvirCam = rooms.GetChild(currentRoom).Find("VirtualCamera");
        currentRoom--;
        currentRoom = Mathf.Clamp(currentRoom, 0, rooms.childCount-1);
        var virCam = rooms.GetChild(currentRoom).Find("VirtualCamera");
        oldvirCam.GetComponent<CinemachineVirtualCamera>().Priority--;
        virCam.GetComponent<CinemachineVirtualCamera>().Priority++;
        updateRoomCount();
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

    //----------------------
    //     In Game State
    //----------------------
    public void BackToMenu()
    {
        currentRoom = 0;
        var virCam = GetCurrentRoom().Find("VirtualCamera");
        virCam.GetComponent<CinemachineVirtualCamera>().Priority = 1;
        player.gameObject.SetActive(false);
        playerVirCam.Priority = -1;
        CurrentGameState(GameStates.Menu);
        updateRoomCount();
    }
    public void Gameover()
    {
        CurrentGameState(GameStates.LoseGame);
        GetCurrentRoomEditor().GetDoor().GetDoorVCam().Priority = 1;
    }
    public void GameWin()
    {
        CurrentGameState(GameStates.WinGame);
    }

    public void CallKeyTaken()
    {
        KeyTaken();
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
        CurrentGameState(GameStates.InGame);
    }
    public bool IsKeyTaken()
    {
        return isKeyTaken;
    }
}
