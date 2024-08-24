using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorController : MonoBehaviour
{
    public Animator animator;

    int _ValueH;
    int _ValueV;

    private void Awake()
    {
        animator = GetComponent<Animator>();

        //get the hash of the parameters to feed to the animator
        _ValueH = Animator.StringToHash("Horizontal");
        _ValueV = Animator.StringToHash("Vertical");
    }

    public void UpdateAnimatorValues(float horizontal, float vertical, bool isSprinting)
    {
        float _snappedHorizontalMovement;
        float _snappedVerticalMovement;

        //Feed the animator with the horizontal values

        #region Snapped Horizontal 
        if (horizontal > 0 && horizontal < 0.5f)
        {
            _snappedHorizontalMovement = 0.5f;
        }
        else if (horizontal > 0.5f)
        {
            _snappedHorizontalMovement = 1f;
        }
        else if (horizontal < 0 && horizontal > -0.5f)
        {
            _snappedHorizontalMovement = -0.5f;
        }
        else if (horizontal < -0.5f)
        {
            _snappedHorizontalMovement = -1f;
        }
        else
        {
            _snappedHorizontalMovement = horizontal;
        }
        #endregion

        //Feed the animator with the vertical values 

        #region Snapped Snapped Vertical 
        if (vertical > 0 && vertical < 0.5f)
        {
            _snappedVerticalMovement = 0.5f;
        }
        else if (vertical > 0.5f)
        {
            _snappedVerticalMovement = 1f;
        }
        else if (vertical < 0 && vertical > -0.5f)
        {
            _snappedVerticalMovement = -0.5f;
        }
        else if (vertical < -0.5f)
        {
            _snappedVerticalMovement = -1f;
        }
        else
        {
            _snappedVerticalMovement = vertical;
        }
        #endregion

        if (isSprinting)
        {
            _snappedHorizontalMovement = horizontal;
            _snappedVerticalMovement = 2; //Value used in BlendTree is used
        }

        animator.SetFloat(_ValueH, _snappedHorizontalMovement, 0.1f, Time.deltaTime);
        animator.SetFloat(_ValueV, _snappedVerticalMovement, 0.1f, Time.deltaTime);
    }

    public void PlayTargetAnimation(string plyAnim, bool isInteracting)
    {
        //Set the 'isInteracting' variable in the animator to the value passed in the isInteracting parameter
        animator.SetBool("isInteracting", isInteracting);

        animator.CrossFade(plyAnim, 0.2f);
    }

}


//What are Regions?
// A region may be used to group a large amount of fields / properties or a couple of methods which belong to a logical subgroup.
// Though don�t use them for single methods or generally too small sections of code.
// You can collapse methods just like regions out of the box.