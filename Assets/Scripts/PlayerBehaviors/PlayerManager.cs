using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerManager : MonoBehaviour
{
    InputManager _inputManager;
    PlayerMovement _playerMovement;
    CameraController _cameraController;
    Animator _animator;
    PhotonView _view;

    public bool isInteracting;

    void Awake()
    {
        _inputManager = GetComponent<InputManager>();
        _playerMovement = GetComponent<PlayerMovement>();
        _cameraController = FindAnyObjectByType<CameraController>();
        _animator = GetComponent<Animator>();
        _view = GetComponent<PhotonView>();
    }

    void Start()
    {
        if (!_view.IsMine)
        {
            Destroy(GetComponentInChildren<CameraController>().gameObject);
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (!_view.IsMine)
        {
            return;
        }

        _inputManager.Handler();
    }

    //Fixed update is called every physics update
    void FixedUpdate()
    {
        if (!_view.IsMine)
        {
            return;
        }
        _playerMovement.Handler();
    }

    void LateUpdate()
    {
        if (!_view.IsMine)
        {
            return;
        }

        _cameraController.CameraHandler();
        isInteracting = _animator.GetBool("isInteracting");
        _playerMovement.isJumping = _animator.GetBool("isJumping");
        _animator.SetBool("isGrounded", _playerMovement.isGrounded);
    }
}
