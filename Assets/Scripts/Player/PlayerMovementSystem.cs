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
    [SerializeField] Transform cameraTarget;

    //-------------------
    // Movement & Forces
    //-------------------
    [SerializeField] float jumpForce;
    [SerializeField] float moveForce;
    [SerializeField] LayerMask floorLayerMask;
    [SerializeField] float disToLand = 4;
    [SerializeField] float maxVelMove = 4;
    [SerializeField] float maxVelJump = 4;
    [SerializeField] float gravity;
    [SerializeField] float lookSen;
    RaycastHit disRay;
    [SerializeField] Vector3 addedVelocity;
    [SerializeField] Vector3 velocity;
    Quaternion playerRotationQ;
    Quaternion cameraTargetRotationQ;
    Vector3 rotateTargetV;
    Vector3 cameraTargetRotationV;
    bool isFloorDetected;

    //---------------------
    // Animator's Controls
    //---------------------
    int runFHash = Animator.StringToHash("RunF");
    int runSHash = Animator.StringToHash("RunS");
    int isMoveHash = Animator.StringToHash("IsMove");
    int isGroundHash = Animator.StringToHash("IsGround");
    int jumpHash = Animator.StringToHash("Jump");
    int readyToLandHash = Animator.StringToHash("ReadyToLand");
    int hitHash = Animator.StringToHash("Hit");
    int inJumpClipHash = Animator.StringToHash("Base Layer.Jump");
    int MoveBlendTreeHash = Animator.StringToHash("Base Layer.Move");
    int landingHash = Animator.StringToHash("Base Layer.Landing");

    bool canMove;
    bool isGround;
    bool readyToLand;
    bool isMoveBlendtree;

    //======================
    //       AWAKE
    //======================
    private void Awake()
    {
        cameraTargetRotationQ = cameraTarget.rotation;
    }
    private void OnEnable()
    {
        isGround = false;
    }

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
        Rotate();
    }

    //========================
    //       MOVEMENT
    //========================
    void PlayerMovement()
    {
        Jump();
        Move();
    }

    //========================
    //       ANIMATION
    //========================
    void AnimatorParametersUpdate()
    {
        animator.SetBool(isMoveHash, canMove);
        animator.SetFloat(runFHash, inputManager.GetMove().y);
        animator.SetFloat(runSHash, inputManager.GetMove().x);
        animator.SetBool(isGroundHash, isGround);
        animator.SetFloat(jumpHash, inputManager.GetJump());
        animator.SetBool(readyToLandHash, readyToLand);

        //Detect Input For "canMove" Boolean
        isMoveBlendtree = GetCurrentClip() == MoveBlendTreeHash || GetCurrentClip() == landingHash;
        canMove = (inputManager.GetMove().magnitude != 0 && isMoveBlendtree);

        //Detect distance From ground before landing animation Starts
        if (!isGround)
        {
            isFloorDetected = Physics.Raycast(transform.position, Vector3.down, out disRay, 50, floorLayerMask);
            readyToLand = isFloorDetected ? disRay.distance < disToLand : false;
        }
        else { readyToLand = true; }
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
    //          CALCULATION
    //===============================
    void Jump()
    {
        if (GetCurrentClip() == inJumpClipHash)
        {
            if (GetNormalizeClipTime() > 0.7f)
            {
                addedVelocity.y = jumpForce * inputManager.GetJump() * Time.fixedDeltaTime;
            }
        }
        else
        {
            addedVelocity.y -= 1f;
        }
    }
    void Move()
    {
        addedVelocity.z = inputManager.GetMove().y * moveForce * Time.fixedDeltaTime;
        addedVelocity.x = inputManager.GetMove().x * moveForce * Time.fixedDeltaTime;
    }
    void Rotate()
    {
        //LEFT & RIGHT Axis Look Rotation
        transform.rotation = playerRotationQ;
        playerRotationQ.eulerAngles = rotateTargetV;
        rotateTargetV.y += inputManager.GetMouse().x * lookSen * Time.deltaTime;

        //UP & DOWN Axis Look Rotation
        cameraTarget.localRotation = cameraTargetRotationQ;
        cameraTargetRotationQ.eulerAngles = cameraTargetRotationV;
        cameraTargetRotationV.x += inputManager.GetMouse().y * lookSen * Time.deltaTime * - 1;
        cameraTargetRotationV.x = Mathf.Clamp(cameraTargetRotationV.x, -25, 80);
    }
    void ForceCalculation()
    {
        //-------------
        //Get Velocity
        //-------------
        velocity = transform.InverseTransformDirection(rb.velocity);

        //----------
        //Constrains
        //----------
        velocity.x = Mathf.Clamp(velocity.x, -maxVelMove, maxVelMove);
        velocity.z = Mathf.Clamp(velocity.z, -maxVelMove, maxVelMove);
        velocity.y = Mathf.Clamp(velocity.y, -Mathf.Infinity, maxVelJump);
        addedVelocity.y = Mathf.Clamp(addedVelocity.y, 0, Mathf.Infinity);
        if (!canMove && isGround)
        {
            velocity.x = 0; velocity.z = 0;
        }

        //---------------
        //Apply Velocity
        //---------------
        velocity += addedVelocity;
        rb.velocity = transform.TransformDirection(velocity);
        rb.AddForce(Physics.gravity * gravity);
    }

    //==========================
    //       PUBLIC METHODS
    //==========================
    public void HurtAnim()
    {
        animator.SetTrigger(hitHash);
    }

    //========================
    //       COLLISIONS
    //========================
    private void OnCollisionStay(Collision collision)
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
    private void OnTriggerEnter(Collider other)
    {

    }

}
