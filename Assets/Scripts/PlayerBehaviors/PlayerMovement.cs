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

    Rigidbody _rb;

    public float speed = 2.0f;
    public float RotateSpeed = 13.0f;

    private void Awake()
    {
        _Camera = Camera.main.transform;
        _inputManager = GetComponent<InputManager>();
        _rb = GetComponent<Rigidbody>();
    }

    public void Movement()
    {
        //get the camera transform, so we can move the player in the direction of the camera but in the x and z axis only
        _Move_dir = _Camera.forward * _inputManager.Input_Vertical;
        _Move_dir = _Move_dir + _Camera.right * _inputManager.Input_Horizontal;
        _Move_dir.Normalize();

        _Move_dir.y = 0;

        _Move_dir = _Move_dir * speed;
        _velocity = _Move_dir;
        _rb.velocity = _velocity;
    }

    public void Rotation()
    {
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

    public void Handler()
    {
        Movement();
        Rotation();
    }
}
