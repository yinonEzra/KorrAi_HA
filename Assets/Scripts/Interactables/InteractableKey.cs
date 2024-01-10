using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableKey : Interactable
{
    [SerializeField] GameManager gameManager;
    [SerializeField] PoolManager_ShineFx shineFxPool;
    [SerializeField] Transform fxPosition;
    protected override void OnPickUp()
    {
        shineFxPool.GetObject(fxPosition.position);
        transform.gameObject.SetActive(false);
        gameManager.CallKeyTaken();
    }
}
