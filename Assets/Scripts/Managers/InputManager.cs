using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    [SerializeField] Vector2 movementAxis;
    [SerializeField] Vector2 mouseAxis;
    [SerializeField] float jump;
    [SerializeField] float esc;
    [SerializeField] bool freezeInputs;

    void Update()
    {
        if (freezeInputs) { return; }
        Move();
        Jump();
        Mouse();
        Esc();
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
    void Esc()
    {
        esc = (Input.GetKeyDown(KeyCode.Escape)) ? esc = 1 : esc = 0;
    }
    //===================================
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
    public float GetEsc()
    {
        return esc;
    }
    //===================================
    //           SET FUNCTIONS
    //===================================
    public void SetFreeze(bool freeze)
    {
        freezeInputs = freeze;
        esc = 0;
        jump = 0;
        movementAxis = Vector2.zero;
        mouseAxis = Vector2.zero;
    }


}
