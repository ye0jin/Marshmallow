using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerHealth : MonoBehaviour
{
    private MeshRenderer[] meshs;

    [SerializeField] private GameObject enemyPref;
    public static PlayerHealth Instance;

    private int maxHealth = 100;
    public int MaxHealth => maxHealth;

    private int currentHealth;
    public int CurrentHealth => currentHealth;

    private bool isDead = false;
    public bool PlayerDead => isDead;

    private EnemyAttack attack;
    private PlayerAnimator animator;

    public UnityEvent DeadTrigger = null;

    private void Awake()
    {
        if(Instance!=null)
        {
            Debug.LogError("Multiple PlayerHealth!");
        }
        Instance = this;
        attack = enemyPref.GetComponent<EnemyAttack>();
        animator = transform.Find("Visual").GetComponent<PlayerAnimator>();

        maxHealth = 100; // 초기화
        currentHealth = maxHealth;

        meshs = GetComponentsInChildren<MeshRenderer>();
    }

    public void AddHealth()
    {
        // 아이템 사서 먹으면... 피 회복
        currentHealth += 10;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        SoundManager.Instance.PlayHeal();
        StartCoroutine(UIManager.Instance.BlinkLifeText(Color.green));
    }

    public void OnDamage(int damage)
    {
        if (isDead) return;
        
        currentHealth -= damage;
        StartCoroutine(DamageBlink());

        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        SoundManager.Instance.PlayPlayerAttackSound();
        StartCoroutine((UIManager.Instance.BlinkLifeText(Color.red)));

        if(currentHealth<=0)
        {
            isDead = true;
            DeadTrigger?.Invoke();
        }
    }

    private IEnumerator DamageBlink()
    {
        foreach(MeshRenderer m in meshs)
        {
            m.material.color = Color.red;
        }

        yield return new WaitForSeconds(0.2f);

        foreach (MeshRenderer m in meshs)
        {
            m.material.color = Color.white;
        }
    }
}
