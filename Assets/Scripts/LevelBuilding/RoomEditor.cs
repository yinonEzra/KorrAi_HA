using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class RoomEditor : MonoBehaviour
{
    //---------------------
    //    Dependencies
    //---------------------
    [SerializeField] Transform floorParent;
    [SerializeField] Transform wallsParent;
    [SerializeField] Transform ceilingParent;
    Transform floorTile;
    Transform wallTile;
    Transform ceilingTile;

    public List<Transform> floors = new List<Transform>();
    public List<Transform> walls = new List<Transform>();
    public List<Transform> ceiling = new List<Transform>();

    public List<Transform> standWalls = new List<Transform>();

    [Range(1,10)]
    [SerializeField] int roomHeight = 0;
    [Range(1, 25)]
    [SerializeField] int roomWidth = 0;
    [Range(1, 25)]
    [SerializeField] int roomDepth = 0;

    Transform standWall;

    public void NewRoomMethod()
    {
        floorParent = transform.Find("Floors");
        floorTile = transform.Find("FloorTileMOD");

        wallsParent = transform.Find("Walls");
        wallTile = transform.Find("WallTileMOD");

        ceilingParent = transform.Find("Ceiling");
        ceilingTile = transform.Find("CeilingTileMOD");
    }
  
    public void SetRoom(int height, int width, int depth)
    {
        //----------
        //  Height
        //----------

        DeleteChildren(wallsParent.gameObject);

        var standWallParent1 = new GameObject("01Wall").transform;
        standWallParent1.SetParent(wallsParent);
        standWallParent1.localPosition = Vector3.zero;
        var standWallParent3 = new GameObject("03Wall").transform;
        standWallParent3.SetParent(wallsParent);
        standWallParent3.localPosition = Vector3.zero;
        var standWall = new GameObject("StandWall").transform;
        standWall.SetParent(standWallParent1);
        standWall.localPosition = Vector3.zero;


        for (int i = 0; i < height; i++)
        {
            var newWall = Instantiate(wallTile, standWall.transform);
            var newPosition = wallTile.position;
            newPosition += (Vector3.up * 15 * i);
            newWall.position = newPosition;
        }
        //----------
        //   Width
        //----------
        for (int i = 0; i < width; i++)
        {
            var newStandWall = Instantiate(standWall, standWallParent1);
            var newPosition = newStandWall.position;
            newPosition += (Vector3.right * 15 * i);
            newStandWall.position = newPosition;
        }
        var standWallParent2 = Instantiate(standWallParent1, wallsParent);
        standWallParent2.name = "02Wall";
        standWallParent2.position += (Vector3.forward * 15 * depth);
        standWallParent2.localRotation  = Quaternion.Euler(Vector3.up * 180);
        standWallParent2.position += (Vector3.right * 15 * width);
        //-----------
        //   Depth
        //-----------
        for (int i = 0; i < depth; i++)
        {
            var newStandWall = Instantiate(standWall, standWallParent3);
            var newPosition = newStandWall.position;
            newPosition += (Vector3.forward * 15 * i);
            newStandWall.position = newPosition;
        }
        var standWallParent4 = Instantiate(standWallParent3, wallsParent);
        standWallParent4.name = "04Wall";
        standWallParent4.position += (Vector3.right * 15 * width);
        standWallParent4.localRotation = Quaternion.Euler(Vector3.up * 180);
        standWallParent4.position += (Vector3.forward * 15 * depth);





        roomHeight = height - 1;
        roomWidth = width - 1;
        roomDepth = depth - 1;

    }




    public void DeleteChildren(GameObject parent)
    {
        List<Transform> children = parent.transform.Cast<Transform>().ToList();
        foreach (Transform child in children)
        {
            GameObject.DestroyImmediate(child.gameObject);
        }
    }

}
