using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    protected void OnTriggerEnter(Collider other)
    {
        OnPickUp();
    }
    abstract protected void OnPickUp();
}
