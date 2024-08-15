using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    InputManager _inputManager;
    PlayerMovement _playerMovement;

    private void Awake()
    {
        _inputManager = GetComponent<InputManager>();
        _playerMovement = GetComponent<PlayerMovement>();
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
}
