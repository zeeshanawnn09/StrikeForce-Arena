using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    PlayerControls _controls;
    AnimatorController _animController;
    PlayerMovement _playerMovement;

    public Vector2 move;
    public Vector2 look;

    public float Input_Horizontal;
    public float Input_Vertical;
    public float movementValue;

    //Axis for the camera
    public float cameraInput_X;
    public float cameraInput_Y;


    [Header("Input Buttons Flags")]

    public bool Sprinting;
    public bool Jumping;

    private void Awake()
    {
        _animController = GetComponent<AnimatorController>();
        _playerMovement = GetComponent<PlayerMovement>();
    }
    void OnEnable()
    {
        if (_controls == null)
        {
            _controls = new PlayerControls();

            //when the player press the move button, the move vector will be updated
            _controls.PlayerMovements.Movements.performed += i => move = i.ReadValue<Vector2>();

            //when the player press the look button, the look vector will be updated
            _controls.PlayerMovements.CameraMovement.performed += i => look = i.ReadValue<Vector2>();

            //When the player press the sprint button, the isSprinting bool will be updated
            _controls.PlayerActions.Sprint.performed += i => Sprinting = true;
            _controls.PlayerActions.Sprint.canceled += i => Sprinting = false;
            _controls.PlayerActions.Jump.performed += i => Jumping = true;
        }

        _controls.Enable();

    }

    void OnDisable()
    {
        _controls.Disable();
    }

    private void MovementHandler()
    {
        //Movement Axis
        Input_Horizontal = move.x;
        Input_Vertical = move.y;

        //Camera Axis
        cameraInput_X = look.x;
        cameraInput_Y = look.y;


        movementValue = Mathf.Clamp01(Mathf.Abs(Input_Horizontal) + Mathf.Abs(Input_Vertical));
        _animController.UpdateAnimatorValues(0, movementValue, _playerMovement.isSprinting);
    }

    private void SprintHandler()
    {
        //if the player is walking and when the SHIFT button is pressed, the player will sprint
        if (Sprinting && movementValue > 0.5f)
        {
            _playerMovement.isSprinting = true;
        }
        else
        {
            _playerMovement.isSprinting = false;
        }
    }

    private void JumpHandler()
    {
        //if the player is jumping, the player will jump and the isJumping bool will be set to false
        if (Jumping)
        {
            Jumping = false;
            _playerMovement.JumpBehavior();
        }
    }

    public void Handler()
    {
        MovementHandler();
        SprintHandler();
        JumpHandler();
    }
}

