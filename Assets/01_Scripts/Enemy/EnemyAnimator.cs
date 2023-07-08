using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimator : MonoBehaviour
{
    private readonly int _isWalking = Animator.StringToHash("Walking");
    private readonly int _isDead = Animator.StringToHash("Dead");
    private readonly int _isAtk = Animator.StringToHash("Attacking");
    private readonly int _isJumpAtk = Animator.StringToHash("Jumping");

    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void SetMove(bool value)
    {
        _animator.SetBool(_isWalking, value);
    }

    public void SetDead(bool value)
    {
        _animator.SetTrigger(_isDead);
    }

    public void SetAtk(bool value)
    {
        _animator.SetBool(_isAtk, value);
    }    

    public void SetJumpAtk(bool value)
    {
        _animator.SetBool(_isJumpAtk, value);
    }    
}
