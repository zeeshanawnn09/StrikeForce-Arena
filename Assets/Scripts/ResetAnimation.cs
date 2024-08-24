using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetAnimation : StateMachineBehaviour
{
    public string isInteract_Bool;
    public bool isInteractStatus;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool(isInteract_Bool, isInteractStatus);
    }

   
}
