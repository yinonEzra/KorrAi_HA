using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableKey : Interactable
{
    
    [SerializeField] Transform fxPosition;

    private void Start()
    {
        Init();
    }
    protected override void OnTrigger(Collider other)
    {
        fxPool.GetObject(fxPosition.position);
        gameManager.CallKeyTaken();
        transform.gameObject.SetActive(false);
    }

    protected override void OnCollision(Collision other)
    {
        //
    }
}
