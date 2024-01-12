using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableHeart : Interactable
{
    void Start()
    {
        Init();
    }

    
    protected override void OnTrigger(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.gameObject.GetComponent<PlayerStats>().AddHeart(1);
            fxPool.GetObject(transform.position);
            transform.gameObject.SetActive(false);
        }
    }


}
