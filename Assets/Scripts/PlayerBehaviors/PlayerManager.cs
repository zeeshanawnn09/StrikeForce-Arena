using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    InputManager _inputManager;
    PlayerMovement _playerMovement;
    CameraController _cameraController;
    Animator _animator;

    public bool isInteracting;

    private void Awake()
    {
        _inputManager = GetComponent<InputManager>();
        _playerMovement = GetComponent<PlayerMovement>();
        _cameraController = FindAnyObjectByType<CameraController>();
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        _inputManager.Handler();
    }

    //Fixed update is called every physics update
    void FixedUpdate()
    {
        _playerMovement.Handler();
    }

    void LateUpdate()
    {
        _cameraController.CameraHandler();
        isInteracting = _animator.GetBool("isInteracting");
        _playerMovement.isJumping = _animator.GetBool("isJumping");
        _animator.SetBool("isGrounded", _playerMovement.isGrounded);
    }
}
