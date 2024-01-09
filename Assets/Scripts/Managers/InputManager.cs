using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    [SerializeField] Vector2 movementAxis;

    int isMoveAnimator = Animator.StringToHash("IsMove");
    bool isMove;


    void Update()
    {
        Move();
    }

    //====================================
    //           PLAYER MOVEMENT
    //===================================
    void Move()
    {
        movementAxis.y = Input.GetAxis("Vertical");
        movementAxis.x = Input.GetAxis("Horizontal");
    }
    public Vector2 GetMove()
    {
        return movementAxis;
    }




}
