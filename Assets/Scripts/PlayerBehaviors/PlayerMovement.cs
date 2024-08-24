using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Vector3 _Move_dir;
    Vector3 _velocity;
    Vector3 _rotation;

    Transform _Camera;

    InputManager _inputManager;
    PlayerManager _playerManager;
    AnimatorController _anim;

    Rigidbody _rb;
    
    [Header("Player Movement Flags")]

    public bool isSprinting;
    public bool isWalking;
    public bool isGrounded;
    public bool isJumping;

    [Header("Player Movement Settings")]

    public float speed = 2.0f;
    public float RotateSpeed = 13.0f;
    public float SprintSpeed = 6.8f;

    [Header("Falling & Landing Settings")]

    public float inAirTimer;
    public float LeapVelocity;
    public float FallVelocity;
    public float RayCastHeightOffSet = 0.5f;

    [Header("Jumping Settings")]

    public float JumpHeight = 3.0f;
    public float GravityIntensity = -3.0f;

    public LayerMask ground_Lyr;

    private void Awake()
    {
        _Camera = Camera.main.transform;
        _inputManager = GetComponent<InputManager>();
        _rb = GetComponent<Rigidbody>();
        _playerManager = GetComponent<PlayerManager>();
        _anim = GetComponent<AnimatorController>();
    }

    public void Movement()
    {
        if (isJumping)
        {
            return;
        }

        //Move the player in the direction of the camera
        _Move_dir = new Vector3(_Camera.forward.x, 0f, _Camera.forward.z) * _inputManager.Input_Vertical;
        _Move_dir = _Move_dir + _Camera.right * _inputManager.Input_Horizontal;
        _Move_dir.Normalize();

        _Move_dir.y = 0;

        if (isSprinting)
        {
            _Move_dir = _Move_dir * SprintSpeed;
        }
        else
        {
            if (_inputManager.movementValue >= 0.5f)
            {
                _Move_dir = _Move_dir * speed;
                isWalking = true;
            }

            if (_inputManager.movementValue >= 0f)
            {
                isWalking = false;
            }
        }
        _velocity = _Move_dir;
        _rb.velocity = _velocity;
    }

    public void Rotation()
    {
        if (isJumping)
        {
            return;
        }

        _rotation = Vector3.zero;

        //rotate the player in the direction of the camera
        _rotation = _Camera.forward * _inputManager.Input_Vertical;
        _rotation = _rotation + _Camera.right * _inputManager.Input_Horizontal;
        _rotation.Normalize();

        _rotation.y = 0;

        if (_rotation == Vector3.zero)
        {
            _rotation = transform.forward;
        }
       
        Quaternion quaternion = Quaternion.LookRotation(_rotation);
        Quaternion Plyr_Rotate = Quaternion.Slerp(transform.rotation, quaternion, RotateSpeed * Time.deltaTime);

        transform.rotation = Plyr_Rotate;
    }

    void FallingAndLanding_Behavior()
    {
        RaycastHit _hit;
        Vector3 RayCastOrigin = transform.position;
        Vector3 targetPos;

        //The raycast will be casted from the center of the player to the ground
        RayCastOrigin.y = RayCastOrigin.y + RayCastHeightOffSet;
        targetPos = transform.position;

        if (!isGrounded && !isJumping)
        {
            //when the player is not on ground play the 'falling' animation
            if (!_playerManager.isInteracting)
            {
                _anim.PlayTargetAnimation("Falling", true);
            }

            //Physics for Falling
            inAirTimer = inAirTimer + Time.deltaTime;
            _rb.AddForce(transform.forward * LeapVelocity);
            _rb.AddForce(-Vector3.up * FallVelocity * inAirTimer);
        }

        if (Physics.SphereCast(RayCastOrigin, 0.2f, -Vector3.up, out _hit, ground_Lyr))
        {
            //when the player lands on the ground play the 'landing' animation
            if (!isGrounded && !_playerManager.isInteracting)
            {
                _anim.PlayTargetAnimation("Landing", true);
            }

            Vector3 RayCastHitPoint = _hit.point;
            targetPos.y = RayCastHitPoint.y;
            inAirTimer = 0;
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }

        if (isGrounded && !isJumping)
        {
            if (_playerManager.isInteracting || _inputManager.movementValue > 0)
            {
                transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime / 0.2f);
            }
            else
            {
                transform.position = targetPos;
            }

        }
    }
    public void JumpBehavior()
    {
        if (isGrounded)
        {
            _anim.animator.SetBool("isJumping", true);
            _anim.PlayTargetAnimation("Jumping", false);

            //Physics for Jumping 
            float jumpVelocity = Mathf.Sqrt(-2 * GravityIntensity * JumpHeight);
            Vector3 plyrVelocity = _Move_dir;

            //when the player jumps, the player will move in the direction of the camera
            plyrVelocity.y = jumpVelocity;
            _rb.velocity = plyrVelocity;

            isJumping = false;
        }
    }

    public void SetisJumping(bool  isJumping)
    {
        this.isJumping = isJumping;
    }

    public void Handler()
    {
        FallingAndLanding_Behavior();

        //when the player is jumping or falling, the player cannot move or rotate
        if (_playerManager.isInteracting)
        {
            return;
        }

        Movement();
        Rotation();
    }
}
