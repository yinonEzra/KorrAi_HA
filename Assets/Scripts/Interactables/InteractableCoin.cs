using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableCoin : Interactable
{
    void Start()
    {
        Init();
    }
    protected override void OnTrigger(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.gameObject.GetComponent<PlayerStats>().AddCoin(10);
            fxPool.GetObject(transform.position);
            transform.gameObject.SetActive(false);
        }
    }

  
    // Update is called once per frame
    void Update()
    {
        
    }
}
