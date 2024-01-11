using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PivotRotate : MonoBehaviour
{
    [SerializeField] protected Transform pivotSource;
    [SerializeField] protected float speed;
    [SerializeField] protected float target;
    [SerializeField] protected Vector3 rotateTargetAxis;
    protected float updated;
    protected Vector3 rotateStart;

    [SerializeField] protected Vector3 updatedPos;

    protected float tFactor = 2;
    protected float tFactorRevesed = 2;
    protected void Init()
    {
        rotateStart = pivotSource.position;
    }
    
    protected virtual void RotationAnim()
    {
        if(tFactor < 1)
        {
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
        }
    }
    public void StartRotate()
    {
        tFactor = 0;
    }
}
