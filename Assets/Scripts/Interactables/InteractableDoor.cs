using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableDoor : Interactable
{
    [SerializeField] Light doorLight;
    public void KeyTaken()
    {
        doorLight.color = Color.green;
    }
    protected override void OnPickUp()
    {

    }

}
