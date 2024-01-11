using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableDoor : Interactable
{
    [SerializeField] Light doorLight;
    [SerializeField] Transform doorPivot;
    [SerializeField] ParticleSystem particleFx;
    [SerializeField] float rotSpeed;
    [SerializeField] Animator anim;
    float tFactor_Door = 1;
    float rotateTarget = 160;
    float rotateStart = 0;
    Vector3 updatedRot;

    private void Update()
    {
        DoorOpenAnim();
    }

    void DoorOpenAnim()
    {
        doorPivot.rotation = Quaternion.Euler(updatedRot);
        if (tFactor_Door < 1)
        {
            tFactor_Door += Time.deltaTime * rotSpeed;
            updatedRot.y = Mathf.SmoothStep(rotateStart, rotateTarget, tFactor_Door);
        }
    }
    public void StartAnimator()
    {
        anim.SetTrigger("OpenDoor");
    }
    public void KeyTaken()
    {
        doorLight.color = Color.green;
        particleFx.Play();
        StartRotateDoor();
    }
    public void StartRotateDoor()
    {
        tFactor_Door = 0;
    }
    protected override void OnPickUp()
    {

    }
    
}
