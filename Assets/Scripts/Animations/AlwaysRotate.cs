using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlwaysRotate : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] Vector3 direction;

    void Update()
    {
        Rotate();
    }

    void Rotate()
    {
        transform.Rotate(direction * speed * Time.deltaTime);
    }
}
