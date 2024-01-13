using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class InteractableDoor : Interactable
{
    [SerializeField] Light doorLight;
    [SerializeField] Transform doorPivot;
    [SerializeField] ParticleSystem particleFx;
    [SerializeField] float rotSpeed;
    [SerializeField] Animator anim;
    [SerializeField] Collider colliderToShutDown;
    [SerializeField] CinemachineVirtualCamera vCam;
    float tFactor_DoorOpen = 2;
    float tFactor_DoorClose = 2;
    float rotateTarget = 160;
    float rotateStart = 0;
    Vector3 updatedRot;

    private void Start()
    {
        Init();
        updatedRot = doorPivot.localRotation.eulerAngles;
    }
    private void Update()
    {
        DoorOpenAnim();
    }

    void DoorOpenAnim()
    {
        doorPivot.localRotation = Quaternion.Euler(updatedRot);
        if (tFactor_DoorOpen < 1)
        {
            tFactor_DoorOpen += Time.deltaTime * rotSpeed;
            updatedRot.y = Mathf.SmoothStep(rotateStart, rotateTarget, tFactor_DoorOpen);
        }
        else if (tFactor_DoorClose < 1)
        {
            tFactor_DoorClose += Time.deltaTime * rotSpeed;
            updatedRot.y = Mathf.SmoothStep(rotateTarget, rotateStart, tFactor_DoorClose);
        }
    }
    protected override void OnTrigger(Collider other)
    {
        if (other.CompareTag("Player") && gameManager.IsKeyTaken())
        {
            CloseDoorAnim();
            gameManager.GameWin();
        }
    }
    //=====================
    //   PUBLIC METHODS
    //=====================

    public void KeyTaken()
    {
        doorLight.color = Color.green;
        particleFx.Play();
        StartOpenDoor();
        colliderToShutDown.enabled = false;
    }
    public void StartOpenDoor()
    {
        tFactor_DoorOpen = 0;
    }
    public void StartCloseDoor()
    {
        tFactor_DoorClose = 0;
    }
    public void OpenDoorAnim()
    {
        anim.SetTrigger("OpenDoor");
    }
    public void CloseDoorAnim()
    {
        anim.SetTrigger("CloseDoor");
    }
    public CinemachineVirtualCamera GetDoorVCam()
    {
        return vCam;
    }
}
