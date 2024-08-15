using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    PlayerControls _controls;
    AnimatorController _animController;

    private float _movementValue;

    public Vector2 move;

    public float Input_Horizontal;
    public float Input_Vertical;

    private void Awake()
    {
        _animController = GetComponent<AnimatorController>();
    }
    void OnEnable()
    {
        if (_controls == null)
        {
            _controls = new PlayerControls();

            //when the player press the move button, the move vector will be updated
            _controls.PlayerMovements.Movements.performed += i => move = i.ReadValue<Vector2>();
        }

        _controls.Enable();

    }

    void OnDisable()
    {
        _controls.Disable();
    }

    private void MovementHandler()
    {
        Input_Horizontal = move.x;
        Input_Vertical = move.y;
        _movementValue = Mathf.Clamp01(Mathf.Abs(Input_Horizontal) + Mathf.Abs(Input_Vertical));
        _animController.UpdateAnimatorValues(0, _movementValue);
    }

    public void Handler()
    {
        MovementHandler();
    }
}

//Continue from 7