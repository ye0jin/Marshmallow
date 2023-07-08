using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 8f, _gravity = -9.8f;

    private CharacterController _characterController;
    private PlayerAnimator _playerAnimator;

    private Vector3 _movementVelocity;
    private float _verticalVelocity;
    private Vector3 _inputVelocity;

    public bool IsMove = true;

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        _playerAnimator = transform.Find("Visual").GetComponent<PlayerAnimator>();
    }

    public void SetInputVelocity(Vector3 val)
    {
        _inputVelocity = val;
    }

    public void SetMovementVelocity(Vector3 value)
    {
        _movementVelocity = value;
        _inputVelocity = value;
    }

    private void CalculatePlayerMovement()
    {
        _movementVelocity = _inputVelocity.normalized * _moveSpeed * Time.fixedDeltaTime;
        _playerAnimator?.SetMove(_movementVelocity.sqrMagnitude > 0);
    }

    private void RotatePlayerTowardsDirection(Vector3 direction)
    {
        if (direction.sqrMagnitude > 0)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 5f * Time.deltaTime);
        }
    }

    private void FixedUpdate()
    {
        if(IsMove) CalculatePlayerMovement();

        if (_characterController.isGrounded)
        {
            _verticalVelocity = _gravity * 0.3f * Time.fixedDeltaTime;
        }
        else
        {
            _verticalVelocity += _gravity * Time.fixedDeltaTime;
        }

        Vector3 move = _movementVelocity + _verticalVelocity * Vector3.up;
        _characterController.Move(move);

        RotatePlayerTowardsDirection(_inputVelocity);
    }
}
