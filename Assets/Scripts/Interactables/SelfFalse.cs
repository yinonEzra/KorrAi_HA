using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfFalse : MonoBehaviour
{
    [SerializeField] float timeToFalse;
    float timer;

    void Update()
    {
        timer += Time.deltaTime;
        if (timer > timeToFalse)
        {
            timer = 0;
            transform.gameObject.SetActive(false);
        }
    }
}
