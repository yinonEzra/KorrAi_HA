using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


public class LevelBuilder : MonoBehaviour
{
    //---------------------
    //    Dependencies
    //---------------------

    [SerializeField] Transform floorTile;
    [SerializeField] Transform wallTile;
    [SerializeField] Transform ceilingTile;
    [SerializeField] Transform roomsParent;
    [SerializeField] GameObject roomTemplate;

    public List<Transform> interactables;
    [HideInInspector]
    public List<Transform> rooms;
    [HideInInspector]
    public int intersIndex;
    [HideInInspector]
    public int roomIndex;
    [HideInInspector]
    public List<string> interactablesS;
    [HideInInspector]
    public List<string> roomsS;
    

    public void RefreshInterList()
    {
        interactablesS = new List<string>();
        for (int i = 0; i < interactables.Count; i++)
        {
            interactablesS.Add(interactables[i].name);
        }
    }
    public void RefreshRoomList()
    {
        rooms = new List<Transform>();
        roomsS = new List<string>();
        for (int i = 0; i < roomsParent.childCount; i++)
        {
            rooms.Add(roomsParent.GetChild(i));
            roomsS.Add(rooms[i].name);
        }
    }
    public void SpawnInteractable()
    {
        Instantiate(interactables[intersIndex]);
    }
    public void NewRoom()
    {
        var newRoom = Instantiate(roomTemplate, roomsParent);
        var roomCount = rooms.Count + 1;
        newRoom.name = "Room " + roomCount;
        newRoom.GetComponent<RoomEditor>().NewRoomMethod();
    }
}





#if UNITY_EDITOR
[CustomEditor(typeof(LevelBuilder))]
public class LevelBuilderEditorOverride : Editor
{
    GUIStyle headLine;
    GUIStyle smallHeadLine;

    int roomHeight;
    int roomWidth;
    int roomDepth;
    public override void OnInspectorGUI()
    {
        headLine = new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter, fontSize = 15, fontStyle = FontStyle.Bold };
        smallHeadLine = new GUIStyle(GUI.skin.label) {fontSize = 12, fontStyle = FontStyle.Bold };
   
        LevelBuilder levelB = (LevelBuilder)target;

        //-----------------
        //  Room Editor
        //-----------------

        EditorGUILayout.LabelField("Room Editor", headLine);
        EditorGUILayout.Space(5f);
        EditorGUILayout.BeginHorizontal();
        GUIContent roomDropList = new GUIContent("Choose Room To Edit");
        levelB.roomIndex = EditorGUILayout.Popup(roomDropList, levelB.roomIndex, levelB.roomsS.ToArray());
        if (GUILayout.Button("New Room", GUILayout.Width(100f)))
        {
            levelB.RefreshRoomList();
            levelB.NewRoom();
            levelB.RefreshRoomList();
        }
        if (GUILayout.Button("Refresh List", GUILayout.Width(100f)))
        {
            levelB.RefreshRoomList();
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.Space(5f);

        EditorGUILayout.LabelField("Room Parameters", smallHeadLine);

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Room Height");
        roomHeight = EditorGUILayout.IntSlider(roomHeight, 1, 10);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Room Width");
        roomWidth = EditorGUILayout.IntSlider(roomWidth, 1, 25);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Room Depth");
        roomDepth = EditorGUILayout.IntSlider(roomDepth, 1, 25);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.Space(5f);

        if (GUILayout.Button("APPLY PARAMETERS",GUILayout.MaxHeight(30)))
        {
            var currentRoom = levelB.rooms[levelB.roomIndex];
            var currentRoomEditor = currentRoom.GetComponent<RoomEditor>();
            currentRoomEditor.SetRoom(roomHeight, roomWidth, roomDepth);
        }

        EditorGUILayout.Space(15f);

        //-------------------------
        //  Interactables Spawner
        //-------------------------
        EditorGUILayout.LabelField("Interactable Spawner", headLine);
        EditorGUILayout.Space(5f);
        EditorGUILayout.BeginHorizontal();
        GUIContent dropList = new GUIContent("Interactables To Spawn");
        levelB.intersIndex = EditorGUILayout.Popup(dropList, levelB.intersIndex, levelB.interactablesS.ToArray());


        if (GUILayout.Button("Refresh List", GUILayout.Width(100f)))
        {
            levelB.RefreshInterList();
        }
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.Space(10f);

        if (GUILayout.Button("SPAWN IT", GUILayout.MaxHeight(30)))
        {
            levelB.SpawnInteractable();
        }
        
        EditorGUILayout.Space(25f);
        EditorGUILayout.LabelField("Script Dependencies", headLine);
        EditorGUILayout.Space(5f);
        base.OnInspectorGUI();
    }




}
#endif
