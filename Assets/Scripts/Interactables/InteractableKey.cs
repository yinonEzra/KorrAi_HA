using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableKey : Interactable
{
    [SerializeField] GameManager gameManager;
    [SerializeField] PoolManager_ShineFx shineFxPool;
    [SerializeField] Transform fxPosition;

    private void Start()
    {
        AttechDependencies();
    }
    public void AttechDependencies()
    {
        gameManager = FindAnyObjectByType<GameManager>();
        shineFxPool = FindAnyObjectByType<PoolManager_ShineFx>();
    }
    protected override void OnPickUp()
    {
        shineFxPool.GetObject(fxPosition.position);
        gameManager.CallKeyTaken();
        transform.gameObject.SetActive(false);
    }
}
