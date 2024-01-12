using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PingPong : MonoBehaviour
{
    [SerializeField] Transform animatedTarget;
    [SerializeField] float distanceAmount;
    [SerializeField] float speed;
    [SerializeField] Vector3 direction;

    [SerializeField] float updated;
    [SerializeField] Vector3 updatedPos;
    [SerializeField] float tFactor = 0;
    [SerializeField] float tFactorReversed = 0;

    void AnimationLoop()
    {
        animatedTarget.localPosition = updatedPos;
        updatedPos = direction * updated;

        if (tFactor < 1)
        {   
            updated = Mathf.SmoothStep(-distanceAmount, distanceAmount, tFactor += Time.deltaTime * speed);
            tFactorReversed = 0;
            return;
        }
        if (tFactorReversed < 1)
        {
            updated = Mathf.SmoothStep(distanceAmount, -distanceAmount, tFactorReversed += Time.deltaTime * speed);
            return;
        }
        tFactor = 0;
    }
    void Update()
    {
        AnimationLoop();
    }


    //==============================
    //        DRAW DANGER ZONE
    //==============================
    void OnDrawGizmos()
    {
        Gizmos.color = new Color(1, 0, 0, 0.5f);
        Gizmos.DrawCube(transform.position, animatedTarget.TransformDirection(Vector3.one + direction * distanceAmount));
    }
}
