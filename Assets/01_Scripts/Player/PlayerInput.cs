using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerInput : MonoBehaviour
{
    [SerializeField] private LayerMask _whatIsGround;
    [SerializeField] private float rollingSpeed = 0.6f;

    private bool isRolling= false;
    public bool IsRolling => isRolling;

    private PlayerAnimator _playerAnimator;
    private PlayerMovement _playerMovement;
    private PlayerHealth _playerHealth;

    private Vector3 playerMove;

    public UnityEvent<Vector3> OnMovementKeyPress = null;
    [SerializeField] private PlayerOneShotWeapon oneshot;
    [SerializeField] private PlayerMachineGunWeapon machine;


    private void Awake()
    {
        oneshot = oneshot.GetComponent<PlayerOneShotWeapon>();
        machine = machine.GetComponent<PlayerMachineGunWeapon>();

        _playerAnimator = transform.Find("Visual").GetComponent<PlayerAnimator>();
        _playerHealth = GetComponent<PlayerHealth>();
        _playerMovement = GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        if (_playerHealth.PlayerDead) return;

        UpdateMoveInput();

        if (Input.GetKeyDown(KeyCode.Space) && !isRolling && !oneshot.IsReloading && !machine.IsReloading) 
        {
            UpdateRolling();
        }
    }

    private void UpdateRolling()
    {
        isRolling = true;
        SoundManager.Instance.PlayRollingSound();

        _playerMovement.IsMove = false;
        _playerAnimator.SetRolling(true);
        Vector3 dir = Quaternion.Euler(0, -45f, 0) * playerMove.normalized;

        if(dir.magnitude<0.1f)
        {
            dir = gameObject.transform.forward;
        }

        _playerMovement.SetMovementVelocity(dir.normalized * rollingSpeed);
        StartCoroutine(WaitForRolling(0.4f));
    }

    public void SetRotation(Quaternion rotation)
    {
        transform.rotation = rotation;
    }

    private void UpdateMoveInput()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        playerMove = new Vector3(h, 0, v);

        OnMovementKeyPress?.Invoke(playerMove);

        Vector3 dir = playerMove.normalized;
        transform.LookAt(transform.position + dir);
    }

    private IEnumerator WaitForRolling(float time)
    {
        yield return new WaitForSeconds(time);
        _playerMovement.IsMove = true;
        _playerAnimator.SetRolling(false);
        isRolling = false;
    }
}
