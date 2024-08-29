using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingBehavior : MonoBehaviour
{
    Animator _animator;
    InputManager _inputManager;
    PlayerMovement _playerMovement;

    [Header("Shooting Settings")]
    
    public Transform firePoint;
    public float fireRate = 0f;
    public float fireRange = 50f;
    public float fireDamage = 10f;

    private float _nextTimeToFire = 0f;

    [Header("Reload Settings")]

    public int maxAmmo = 15;
    public float reloadTime = 1.5f;

    private int currAmmo;

    [Header("Sound Settings")]

    public AudioSource Audio;
    public AudioClip shootSound;
    public AudioClip reloadSound;

    [Header("Shooting Flags")]

    public bool isFiring;
    public bool isShootInput;
    public bool isWalking;
    public bool isReloading = false;


    // Start is called before the first frame update
    private void Start()
    {
        _animator = GetComponent<Animator>();
        _inputManager = GetComponent<InputManager>();
        _playerMovement = GetComponent<PlayerMovement>();
        currAmmo = maxAmmo;
    }

    private void Shoot()
    {
        if (currAmmo > 0)
        {
            RaycastHit _hit;
            if (Physics.Raycast(firePoint.position, firePoint.forward, out _hit, fireRange))
            {
                Debug.Log(_hit.transform.name);
            }

            //play the shoot sound and decrease the current ammo
            Audio.PlayOneShot(shootSound);
            currAmmo--;
        }
        else
        {
            Reload();
        }
    }

    private void Reload()
    {

        if (!isReloading && currAmmo < maxAmmo)
        {
            if (isShootInput && isWalking)
            {
                _animator.SetTrigger("ShootReload");
            }
            else
            {
                _animator.SetTrigger("Reload");
            }

            isReloading = true;
            Audio.PlayOneShot(reloadSound);
            Invoke("FinishReloading", reloadTime);
        }
    }

    private void FinishReloading()
    {
        currAmmo = maxAmmo;
        isReloading = false;

        if (isShootInput && isWalking)
        {
            _animator.ResetTrigger("ShootReload");
        }
        else
        {
            _animator.ResetTrigger("Reload");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(isReloading || _playerMovement.isSprinting)
        {
            _animator.SetBool("canShoot", false);
            _animator.SetBool("ShootMovement", false);
            _animator.SetBool("ShootWalk", false);
            return;
        }
        isWalking = _playerMovement.isWalking;
        isShootInput = _inputManager.Fire_Input;

        if (isShootInput && isWalking)
        {
            //if the player is walking and shooting, the player will shoot and the shootwalk animation will be played
            if (Time.time >= _nextTimeToFire)
            {
                _nextTimeToFire = Time.time + 1f / fireRate;
                Shoot();
                _animator.SetBool("ShootWalk", true);
            }

            _animator.SetBool("canShoot", false);
            _animator.SetBool("ShootMovement", true);
            isFiring = true;

        }

        else if (isShootInput)
        {
            //if the player is shooting, the player will shoot and the shoot animation will be played
            if (Time.time >= _nextTimeToFire)
            {
                _nextTimeToFire = Time.time + 1f / fireRate;
                Shoot(); 
            }

            _animator.SetBool("canShoot", true);
            _animator.SetBool("ShootMovement", false);
            _animator.SetBool("ShootWalk", false);
            isFiring = true;

        }

        else
        {
            _animator.SetBool("canShoot", false);
            _animator.SetBool("ShootMovement", false);
            _animator.SetBool("ShootWalk", false);
            isFiring = false;
        }

        //if 'R' is pressed, the player will reload
        if (_inputManager.Reload_Input && currAmmo < maxAmmo)
        {
            Reload();
        }   
    }   
}
