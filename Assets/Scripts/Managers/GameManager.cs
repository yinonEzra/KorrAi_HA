using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //--------------
    // Dependencies
    //--------------
    [SerializeField] InteractableKey key;
    [SerializeField] InteractableDoor door;
    


    void KeyTaken()
    {
        door.KeyTaken();
    }



    //======================
    //    PUBLIC METHODS
    //======================
    public void CallKeyTaken()
    {
        KeyTaken();
    }
}
