using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PivotRotateCeiling : PivotRotate
{
    [SerializeField] CinemachineVirtualCamera vCam;

    void Start()
    {
        Init();
    }

    void Update()
    {
        RotationAnim();
    }

    protected override void RotationAnim()
    {
        if (tFactor < 1)
        {
            vCam.Priority = 2;

            pivotSource.localRotation = Quaternion.Euler(updatedPos);
            tFactor += Time.deltaTime * speed;
            updated = Mathf.SmoothStep(0, target, tFactor);
            updatedPos = rotateTargetAxis * updated;

            tFactorRevesed = 0;
        }
        else if (tFactorRevesed < 1)
        {
            pivotSource.localRotation = Quaternion.Euler(updatedPos);
            tFactorRevesed += Time.deltaTime * speed;
            updated = Mathf.SmoothStep(target, 0, tFactorRevesed);
            updatedPos = rotateTargetAxis * updated;
            vCam.Priority = 0;

        }
    }

}
