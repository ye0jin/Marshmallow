using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    private readonly int _isWalking = Animator.StringToHash("Walking");
    private readonly int _isShooting = Animator.StringToHash("Shooting");
    private readonly int _isReloading = Animator.StringToHash("Reloading");
    private readonly int _isRolling = Animator.StringToHash("Rolling");
    private readonly int _isDead = Animator.StringToHash("Dead");


    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void SetMove(bool value)
    {
        _animator.SetBool(_isWalking, value);
    }

    public void SetRolling(bool value)
    {
        _animator.SetBool(_isRolling, value);
    }

    public void SetReload(bool value)
    {
        _animator.SetBool(_isReloading, value);
    }

    public void SetShoot(bool value)
    {
        _animator.SetBool(_isShooting, value);
    }

    public void SetDead()
    {
        _animator.SetTrigger(_isDead);
    }
}
