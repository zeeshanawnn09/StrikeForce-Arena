using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private InputManager _inputManager;

    private Vector3 _CameraVelocity = Vector3.zero;


    public Transform player;
    public Transform cameraTransform;

    [Header("Camera Movement Settings")]

    public Transform camera;

    public float CameraSmoothTime = 0.3f;
    public float HorizontalLook;
    public float H_LookSpeed = 2.0f;
    public float VerticalLook;
    public float V_LookSpeed = 2.0f;
    public float MinLookAngle = -30.0f;
    public float MaxLookAngle = 30.0f;

    [Header("Camera Collision Settings")]

    private float _DefaultPosCamera;

    private Vector3 _CameraPos;

    public LayerMask CameraCollisionLayer;

    public float CameraCollisionOffset = 0.2f;
    public float MinCollisionOffset = 0.2f;
    public float CameraCollisionRadius = 0.2f;





    // Awake is called when the script instance is being loaded
    private void Awake()
    {
        //Lock the cursor in the center of the screen and hide it
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        _inputManager = FindAnyObjectByType<InputManager>();
        player = FindAnyObjectByType<PlayerManager>().transform;

        //Get the camera transform and the default position of the camera
        cameraTransform = Camera.main.transform;
        _DefaultPosCamera = cameraTransform.localPosition.z;
    }

    void FollowPlayer()
    {
        Vector3 playerPos = Vector3.SmoothDamp(transform.position, player.position, ref _CameraVelocity, CameraSmoothTime);
        transform.position = playerPos;
    }

    void RotateCamera()
    {
        Vector3 _rotation;
        Quaternion _targetRotation;

        HorizontalLook = HorizontalLook + (_inputManager.cameraInput_X * H_LookSpeed);
        VerticalLook = VerticalLook + (_inputManager.cameraInput_Y * V_LookSpeed);
        VerticalLook = Mathf.Clamp(VerticalLook, MinLookAngle, MaxLookAngle);

        _rotation = Vector3.zero;
        _rotation.y = HorizontalLook;

        //Clamp the horizontal look to avoid the camera to rotate 360 degrees
        _targetRotation = Quaternion.Euler(_rotation);
        transform.rotation = _targetRotation;

        //Clamp the vertical look to avoid the camera to rotate 360 degrees
        _rotation = Vector3.zero;
        _rotation.x = VerticalLook;
        _targetRotation = Quaternion.Euler(_rotation);
        camera.localRotation = _targetRotation;

    }

    void CameraCollision()
    {
        float _plyrPos = _DefaultPosCamera;
        RaycastHit hit;
        Vector3 direction = cameraTransform.position - camera.position;
        direction.Normalize();

        //Check if the camera is colliding with any object in the scene
        if (Physics.SphereCast(camera.transform.position, CameraCollisionRadius, direction, out hit, Mathf.Abs(_plyrPos), CameraCollisionLayer))
        {
            //Get the distance between the camera and the object that the camera is colliding with and adjust the camera position to avoid the collision
            float distance = Vector3.Distance(camera.position, hit.point);
            _plyrPos =- (distance - CameraCollisionOffset);
        }

        if (Mathf.Abs(_plyrPos) < MinCollisionOffset)
        {
            _plyrPos = _plyrPos - MinCollisionOffset;
        }

        //Smoothly move the camera to the new position to avoid a sudden camera movement 
        _CameraPos.z = Mathf.Lerp(cameraTransform.localPosition.z, _plyrPos, 0.1f);
        cameraTransform.localPosition = _CameraPos;

    }

    //Handles the camera movement
    public void CameraHandler()
    {
        FollowPlayer();
        RotateCamera();
        CameraCollision();
    }
}

