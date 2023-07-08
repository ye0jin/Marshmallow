using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BossHealth : MonoBehaviour
{
    private bool isDead = false;
    public bool IsDead => isDead;

    private float maxHealth = 200;
    private float currentHealth = 0;
    public float CurrentHealth => currentHealth;

    private float currentScale=1;

    private EnemyAnimator animator;

    [SerializeField] private Transform fillHP;

    public UnityEvent OnDead = null;

    private void Awake()
    {
        animator = transform.Find("Visual").GetComponent<EnemyAnimator>();

        isDead = false;
        currentHealth = maxHealth; // 초기 설정
    }

    public void OnDamage(float damage)
    {
        currentHealth -= damage;
        float value = damage / maxHealth;
        fillHP.localScale = new Vector3(currentScale - value, 1, 1);


        currentScale -= value;

        if (currentHealth<=0)
        {
            isDead = true;
            animator.SetDead(true);
            OnDead?.Invoke();
        }
    }
}
