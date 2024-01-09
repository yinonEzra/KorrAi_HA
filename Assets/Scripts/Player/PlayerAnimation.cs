using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    //--------------
    // Dependencies
    //--------------
    [SerializeField] InputManager inputManager;
    [SerializeField] Animator animator;

    //---------------------
    // Animator's Controls
    //---------------------
    int runFHash = Animator.StringToHash("RunF");
    int runSHash = Animator.StringToHash("RunS");
    int isMoveAnimator = Animator.StringToHash("IsMove");

    bool isMove;

    private void Update()
    {
        PlayerMovement();
        AnimatorControls();
    }

    void PlayerMovement()
    {
        
    }

    void AnimatorControls()
    {
        animator.SetBool(isMoveAnimator, isMove);
        animator.SetFloat(runFHash, inputManager.GetMove().y);
        animator.SetFloat(runSHash, inputManager.GetMove().x);

        if(inputManager.GetMove().y !=0 || inputManager.GetMove().x != 0)
        {
            isMove = true;
        }
        else
        {
            isMove = false;
        }
    }

}
