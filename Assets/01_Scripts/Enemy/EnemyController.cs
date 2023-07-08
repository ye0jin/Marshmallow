using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public enum EnemyState
{
    Idle,
    Moving,
    Attacking
}

public class EnemyController : MonoBehaviour
{
    [SerializeField] private GameObject target; // 플레이어의 Transform 컴포넌트
    
    private EnemyState currentState;
    public EnemyState CurrentState => currentState;

    [SerializeField] private float moveSpeed = 5f; // 이동 속도
    private float attackRange = 5f; // 공격 범위
    private float followRange = 30f; // 따라가는 범위

    private EnemyAnimator animator;
    public EnemyAnimator Animator => animator;

    private float animationLength = 2f;
    private bool isAttacking = false;

    protected void Awake()
    {
        currentState = EnemyState.Idle; // 초기 상태를 Idle로 설정

        target = GameObject.Find("Player");
        animator = transform.Find("Visual").GetComponent<EnemyAnimator>();
    }

    private void Update()
    {
        //Debug.Log(currentState);
        float distanceToTarget = Vector3.Distance(transform.position, target.transform.position);

        
        switch (currentState)
        {
            case EnemyState.Idle:
                Idle();
                if (distanceToTarget <= followRange)
                {
                    currentState = EnemyState.Moving;
                }
                break;

            case EnemyState.Moving:
                Move();
                if (distanceToTarget <= attackRange)
                {
                    currentState = EnemyState.Attacking;
                    isAttacking = true;
                }
                else if (distanceToTarget > followRange)
                {
                    currentState = EnemyState.Idle;
                }
                break;

            case EnemyState.Attacking:
                if (isAttacking)
                {
                    Attack();
                    Invoke(nameof(EndAttack), animationLength);
                }
                if (distanceToTarget > attackRange)
                {
                    currentState = EnemyState.Moving;
                }
                break;
        }
        
    }

    private void Idle()
    {
        animator.SetMove(false);
    }

    private void Move()
    {
        animator.SetMove(true);

        if(!isAttacking)
        {
            Quaternion targetRotation = Quaternion.LookRotation(target.transform.position - transform.position);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
        }

        transform.position += transform.forward * moveSpeed * Time.deltaTime;
    }

    private void Attack()
    {
        animator.SetAtk(true);
        //Debug.Log("Fire");
    }

    private void EndAttack()
    {
        animator.SetAtk(false);
        isAttacking = false;
        //Debug.Log("끝");
    }
}
