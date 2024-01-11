using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


public class LevelBuilder : MonoBehaviour
{
    //---------------------
    //    Dependencies
    //---------------------
    [SerializeField] Transform roomsParent;
    [SerializeField] GameObject roomTemplate;
    [SerializeField] Transform platform;

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
    public void SpawnPlatform()
    {
        var newPlatform = Instantiate(platform);
    }
    public void SpawnInteractable()
    {
        var currentRoom = rooms[roomIndex];
        var spawnedObject = interactables[intersIndex];
        var roomEditor = currentRoom.GetComponent<RoomEditor>();
        var spawnedParent = roomEditor.interactablesgParent;
        if (spawnedObject.name == "Heart")
        {
            spawnedParent = spawnedParent.Find("Hearts");
            PrefabUtility.InstantiatePrefab(spawnedObject, spawnedParent);
            print("Heart Spawned");
        }
        else if(spawnedObject.name == "Coin")
        {
            spawnedParent = spawnedParent.Find("Coins");
            PrefabUtility.InstantiatePrefab(spawnedObject, spawnedParent);
            print("Coin Spawned");
        }
        else if(spawnedObject.name == "Door")
        {
            if (spawnedParent.Find("Door") != null)
            {
                print("Door Already Exist");
            }
            else
            {
                PrefabUtility.InstantiatePrefab(spawnedObject, spawnedParent);
                print("Door Spawned");
            }
        }
        else if (spawnedObject.name == "Key")
        {
            if (spawnedParent.Find("Key") != null)
            {
                print("Key Already Exist");
            }
            else
            {
                var key = PrefabUtility.InstantiatePrefab(spawnedObject, spawnedParent) as GameObject;
                print("Key Spawned");
            }
        }

    }
    public void NewRoom(int height, int width, int depth)
    {
        var newRoom = Instantiate(roomTemplate, roomsParent);
        var roomCount = rooms.Count + 1;
        newRoom.name = "Room " + roomCount;
        newRoom.GetComponent<RoomEditor>().NewRoomMethod(height, width, depth);
        if (rooms.Count > 0)
        {
            newRoom.transform.position = rooms[rooms.Count - 1].position + (Vector3.forward * 200);
        }
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
    float platformRadius;
    public override void OnInspectorGUI()
    {
        headLine = new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter, fontSize = 15, fontStyle = FontStyle.Bold };
        smallHeadLine = new GUIStyle(GUI.skin.label) {fontSize = 12, fontStyle = FontStyle.Bold };
   
        LevelBuilder levelB = (LevelBuilder)target;

        //=================
        //  Room Editor
        //=================

        EditorGUILayout.LabelField("Room Editor", headLine);
        EditorGUILayout.Space(5f);
        EditorGUILayout.BeginHorizontal();
        GUIContent roomDropList = new GUIContent("Choose Room To Edit");
        levelB.roomIndex = EditorGUILayout.Popup(roomDropList, levelB.roomIndex, levelB.roomsS.ToArray());
        
        if (GUILayout.Button("Refresh List", GUILayout.Width(100f)))
        {
            levelB.RefreshRoomList();
        }
        EditorGUILayout.EndHorizontal();
        
        EditorGUILayout.Space(5f);

        EditorGUILayout.LabelField("Room Parameters", smallHeadLine);

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Room Height");
        roomHeight = EditorGUILayout.IntSlider(roomHeight, 1, 8);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Room Width");
        roomWidth = EditorGUILayout.IntSlider(roomWidth, 1, 8);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Room Depth");
        roomDepth = EditorGUILayout.IntSlider(roomDepth, 1, 8);
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.Space(5f);

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("UPDATE CURRENT ROOM",GUILayout.MaxHeight(30)))
        {
            var currentRoom = levelB.rooms[levelB.roomIndex];
            var currentRoomEditor = currentRoom.GetComponent<RoomEditor>();
            currentRoomEditor.SetRoom(roomHeight, roomWidth, roomDepth);
        }

        if (GUILayout.Button("Create New Room", GUILayout.MaxHeight(30), GUILayout.Width(150)))
        {
            levelB.RefreshRoomList();
            levelB.NewRoom(roomHeight, roomWidth, roomDepth);
            levelB.RefreshRoomList();
        }

        EditorGUILayout.EndHorizontal();

        EditorGUILayout.Space(30f);

        //=========================
        //  Interactables Spawner
        //=========================
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

        //==========================
        //     Platform Spawner
        //==========================
        EditorGUILayout.LabelField("Platform Spawner", headLine);
        EditorGUILayout.Space(5f);
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Platform Radius");
        platformRadius = EditorGUILayout.Slider(platformRadius, 1, 10);
        EditorGUILayout.EndHorizontal();
        if (GUILayout.Button("SPAWN IT", GUILayout.MaxHeight(30)))
        {
            levelB.SpawnInteractable();
        }

    }




}
#endif
