using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    [SerializeField] protected GameManager gameManager;
    [SerializeField] protected PoolManager fxPool;

    protected void OnTriggerEnter(Collider other)
    {
        OnTrigger(other);
    }
    private void OnCollisionEnter(Collision collision)
    {
        OnCollision(collision);
    }
    protected virtual void OnCollision(Collision other)
    {

    }

    protected virtual void OnTrigger(Collider other) 
    {

    }

    public virtual void Init()
    {
        gameManager = FindAnyObjectByType<GameManager>();
        fxPool = FindAnyObjectByType<PoolManager_ShineFx>();
    }
}
