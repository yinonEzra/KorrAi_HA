using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    [SerializeField] Vector2 movementAxis;
    [SerializeField] Vector2 mouseAxis;
    [SerializeField] float jump;

    void Update()
    {
        Move();
        Jump();
        Mouse();
    }

    //=====================================
    //           INPUT DETECTION
    //=====================================
    void Move()
    {
        movementAxis.y = Input.GetAxis("Vertical");
        movementAxis.x = Input.GetAxis("Horizontal");

    }
    void Jump()
    {
        jump = (Input.GetKey(KeyCode.Space)) ? jump = 1 : jump = 0;
    }
    void Mouse()
    {
        mouseAxis.x = Input.GetAxis("Mouse X");
        mouseAxis.y = Input.GetAxis("Mouse Y");
    }
    //====================================
    //           GET FUNCTIONS
    //===================================
    public Vector2 GetMove()
    {
        return movementAxis;
    }
    public float GetJump()
    {
        return jump;
    }
    public Vector2 GetMouse()
    {
        return mouseAxis;
    }




}
