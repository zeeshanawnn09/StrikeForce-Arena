using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorController : MonoBehaviour
{
    Animator _anim;

    int _ValueH;
    int _ValueV;

    private void Awake()
    {
        _anim = GetComponent<Animator>();

        //get the hash of the parameters to feed to the animator
        _ValueH = Animator.StringToHash("Horizontal");
        _ValueV = Animator.StringToHash("Vertical");
    }

    public void UpdateAnimatorValues(float horizontal, float vertical)
    {
        _anim.SetFloat(_ValueH, horizontal, 0.1f, Time.deltaTime);
        _anim.SetFloat(_ValueV, vertical, 0.1f, Time.deltaTime);
    }
}
