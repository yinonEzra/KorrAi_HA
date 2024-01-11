using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class RoomEditor : MonoBehaviour
{
    //---------------------
    //    Dependencies
    //---------------------
    public Transform floorParent;
    public Transform wallsParent;
    public Transform ceilingParent;
    public Transform interactablesgParent;

    Transform floorTile;
    Transform wallTile;
    Transform ceilingTile;

    //===========================
    //    ROOM CONTROL METHODS 
    //===========================
    public void NewRoomMethod(int height, int width, int depth)
    {
        floorParent = transform.Find("Floors");
        floorTile = transform.Find("FloorTileMOD");

        wallsParent = transform.Find("Walls");
        wallTile = transform.Find("WallTileMOD");

        ceilingParent = transform.Find("Ceiling");
        ceilingTile = transform.Find("CeilingTileMOD");
        SetRoom(height, width, depth);
    }
    public void SetRoom(int height, int width, int depth)
    {
        GenerateWalls(height, width, depth);
        GenerateFloor(width, depth - 1);
        GenerateCeiling(height, width, depth - 1);

    }

    //=======================
    //    ROOM GENERATORS 
    //=======================
    private void GenerateWalls(int height, int width, int depth)
    {
        wallTile.gameObject.SetActive(true);
        DeleteChildren(wallsParent.gameObject);

        //----------
        //  Height
        //----------
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
        standWallParent2.localRotation = Quaternion.Euler(Vector3.up * 180);
        standWallParent2.position += (Vector3.right * 15 * width);
        //-----------
        //   Depth
        //-----------
        for (int i = 0; i < depth; i++)
        {
            var newStandWall = Instantiate(standWall, standWallParent3);
            newStandWall.localRotation = Quaternion.Euler(Vector3.up * 90);
            var newPosition = newStandWall.position + (Vector3.forward * 15);
            newPosition += (Vector3.forward * 15 * i);
            newStandWall.position = newPosition;
        }
        var standWallParent4 = Instantiate(standWallParent3, wallsParent);
        standWallParent4.name = "04Wall";
        standWallParent4.position += (Vector3.right * 15 * width);
        standWallParent4.localRotation = Quaternion.Euler(Vector3.up * 180);
        standWallParent4.position += (Vector3.forward * 15 * depth);

        wallTile.gameObject.SetActive(false);
    }
    private void GenerateFloor(int width, int depth)
    {
        floorTile.gameObject.SetActive(true);
        DeleteChildren(floorParent.gameObject);

        var stripFloor = new GameObject("StripFloor").transform;
        stripFloor.SetParent(floorParent);
        stripFloor.localPosition = Vector3.zero;

        //----------
        //   Width
        //----------
        for (int i = 0; i < width; i++)
        {
            var newFloor = Instantiate(floorTile, stripFloor.transform);
            var newPosition = floorTile.position;
            newPosition += (Vector3.right * 15 * i);
            newFloor.position = newPosition;
        }

        //-----------
        //   Depth
        //-----------
        for (int i = 0; i < depth; i++)
        {
            var newStripFloor = Instantiate(stripFloor, floorParent);
            var newPosition = newStripFloor.position + (Vector3.forward * 15);
            newPosition += (Vector3.forward * 15 * i);
            newStripFloor.position = newPosition;
        }

        floorTile.gameObject.SetActive(false);
    }
    private void GenerateCeiling(int height, int width, int depth)
    {
        ceilingTile.gameObject.SetActive(true);
        DeleteChildren(ceilingParent.gameObject);

        var stripCeiling = new GameObject("StripCeiling").transform;
        stripCeiling.SetParent(ceilingParent);
        stripCeiling.localPosition = Vector3.zero;

        //----------
        //   Width
        //----------
        for (int i = 0; i < width; i++)
        {
            var newCeiling = Instantiate(ceilingTile, stripCeiling.transform);
            var newPosition = ceilingTile.position;
            newPosition += (Vector3.right * 15 * i);
            newCeiling.position = newPosition;
        }

        //-----------
        //   Depth
        //-----------
        for (int i = 0; i < depth; i++)
        {
            var newStripCeiling = Instantiate(stripCeiling, ceilingParent);
            var newPosition = newStripCeiling.position + (Vector3.forward * 15);
            newPosition += (Vector3.forward * 15 * i);
            newStripCeiling.position = newPosition;
        }
        //------------
        //   Height
        //------------
        for (int i = 0; i < ceilingParent.childCount; i++)
        {
            var ceilingStrip = ceilingParent.GetChild(i);
            ceilingStrip.position += Vector3.up * 15 * (height - 1);
        }

        ceilingTile.gameObject.SetActive(false);
    }
    void SetPlayerPos()
    {

    }
    private void SetMainLightPos()
    {

    }
    public void DeleteChildren(GameObject parent)
    {
        List<Transform> children = parent.transform.Cast<Transform>().ToList();
        foreach (Transform child in children)
        {
            GameObject.DestroyImmediate(child.gameObject);
        }
    }

    //=======================
    //    PUBLIC METHODS 
    //=======================

    public InteractableKey GetKey()
    {
        return interactablesgParent.GetComponentInChildren<InteractableKey>();
    }
    public InteractableDoor GetDoor()
    {
        return interactablesgParent.GetComponentInChildren<InteractableDoor>();
    }
}
