using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObstacle : Interactable
{

    void Start()
    {
        Init();
    }
    protected override void OnCollision(Collision other)
    {
        if (other.collider.CompareTag("Player"))
        {
            other.collider.GetComponent<PlayerStats>().LoseHeart();
            fxPool.GetObject(other.GetContact(0).point);
        }
    }
    public override void Init()
    {
        gameManager = FindAnyObjectByType<GameManager>();
        fxPool = FindAnyObjectByType<PoolManager_HurtFx>();
    }
}
