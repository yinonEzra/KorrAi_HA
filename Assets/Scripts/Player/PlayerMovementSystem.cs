using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementSystem : MonoBehaviour
{
    //--------------
    // Dependencies
    //--------------
    [SerializeField] InputManager inputManager;
    [SerializeField] Animator animator;
    [SerializeField] Rigidbody rb;
    [SerializeField] CapsuleCollider col;

    //-----------------
    // Movement & Forces
    //-----------------

    [SerializeField] float jumpForce;
    [SerializeField] float moveForce;
    [SerializeField] LayerMask floorLayerMask;
    [SerializeField] float disToLand = 4;
    [SerializeField] float maxVelMove = 4;
    [SerializeField] float maxVelJump = 4;
    [SerializeField] float gravity;
    RaycastHit disRay;
    [SerializeField] Vector3 addedVelocity;
    [SerializeField] Vector3 velocity;

    //---------------------
    // Animator's Controls
    //---------------------
    int runFHash = Animator.StringToHash("RunF");
    int runSHash = Animator.StringToHash("RunS");
    int isMoveHash = Animator.StringToHash("IsMove");
    int isGroundHash = Animator.StringToHash("IsGround");
    int jumpHash = Animator.StringToHash("Jump");
    int readyToLandHash = Animator.StringToHash("ReadyToLand");
    int InJumpClipHash = Animator.StringToHash("Base Layer.Jump");

    bool isMove;
    bool isGround;
    bool readyToLand;

    //======================
    //       UPDATE
    //======================

    private void FixedUpdate()
    {
        PlayerMovement();
        ForceCalculation();
    }
    private void Update()
    {
        AnimatorParametersUpdate();
    }

    //========================
    //       MOVEMENT
    //========================

    void PlayerMovement()
    {
        Jump();
        Move();
    }

    void AnimatorParametersUpdate()
    {
        animator.SetBool(isMoveHash, isMove);
        animator.SetFloat(runFHash, inputManager.GetMove().y);
        animator.SetFloat(runSHash, inputManager.GetMove().x);
        animator.SetBool(isGroundHash, isGround);
        animator.SetFloat(jumpHash, inputManager.GetJump());
        animator.SetBool(readyToLandHash, readyToLand);

        //Detect Input For "isMove" Boolean
        isMove = (inputManager.GetMove().y != 0 || inputManager.GetMove().x != 0);

        //Detect distance From ground before landing animation Starts
        if (!isGround)
        {
            if (Physics.Raycast(transform.position, Vector3.down, out disRay, 50, floorLayerMask))
            {
                readyToLand = (disRay.distance < disToLand);
            }
        }
    }
    int GetCurrentClip()
    {
        return animator.GetCurrentAnimatorStateInfo(0).fullPathHash;
    }
    float GetNormalizeClipTime()
    {
        return animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
    }

    //===============================
    //       FORCES CALCULATION
    //===============================
    void Jump()
    {
        if (GetCurrentClip() == InJumpClipHash && isGround)
        {
            if (GetNormalizeClipTime() > 0.85f)
            {
                addedVelocity.y = jumpForce;
            }
        }
        else
        {
            addedVelocity.y -= 1f;
        }
        col.height = readyToLand ? 3f : 2f;
        col.center = readyToLand ? Vector3.up * 1.5f : Vector3.up * 2f;
    }
    void Move()
    {
        addedVelocity.z = inputManager.GetMove().y * moveForce * Time.fixedDeltaTime;
        addedVelocity.x = inputManager.GetMove().x * moveForce * Time.fixedDeltaTime;
    }
    void Rotate()
    {

    }
    void ForceCalculation()
    {
        velocity = rb.velocity;

        //----------
        //Constrains
        //----------
        velocity.x = Mathf.Clamp(velocity.x, -maxVelMove, maxVelMove);
        velocity.z = Mathf.Clamp(velocity.z, -maxVelMove, maxVelMove);
        velocity.y = Mathf.Clamp(velocity.y, -Mathf.Infinity, maxVelJump);
        addedVelocity.y = Mathf.Clamp(addedVelocity.y, 0, Mathf.Infinity);


        velocity += addedVelocity;
        rb.velocity = velocity;
        rb.AddForce(Physics.gravity * gravity);

        //rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxVel);
    }

    //========================
    //       COLLISIONS
    //========================

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Floor"))
        {
            isGround = true;
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.collider.CompareTag("Floor"))
        {
            isGround = false;
        }
    }

}
