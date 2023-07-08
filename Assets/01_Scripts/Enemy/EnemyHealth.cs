using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private ParticleSystem deadParticle;

    private int maxHealth = 20;
    public int MaxHealth => maxHealth;
    private int currentHealth;

    private EnemyAnimator _animator;

    private bool isDead = false;

    public UnityEvent OnDead = null;

    private void Awake()
    {
        _animator = transform.Find("Visual").GetComponent<EnemyAnimator>();

        currentHealth = maxHealth;
    }

    private void Update()
    {
        if (isDead)
        {
            return;
        }
    }

    public void OnDamage(int damage)
    {
        if (isDead)
        {
            return;
        }

        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        //Debug.Log(currentHealth);
        if (currentHealth <= 0)
        {
            isDead = true;
            UIManager.Instance.EnemyKill();
            OnDead?.Invoke();
            Destroy(gameObject, 5f);
        }
    }

    public void DieParticle()
    {
        ParticleSystem particle = Instantiate(deadParticle, transform.position, Quaternion.identity);
        particle.Play();
        Destroy(particle.gameObject, particle.main.duration);
    }
}
