using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Attack
{
    Shooting=1,
    JumpAtk=2
}

public class Boss : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float speed = 5f;
    [SerializeField] private GameObject atkBulletPref;

    [SerializeField] private Transform pos1;
    [SerializeField] private Transform pos2;

    private BossHealth bossHealth;
    private EnemyAnimator animator;
    private Rigidbody rigid;

    private bool isAttack = true;

    private float nextAtkTime = 5.5f;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        animator = transform.Find("Visual").GetComponent<EnemyAnimator>();
        bossHealth = GetComponent<BossHealth>();
    }

    private void Update()
    {
        if (!CameraManager.Instance.BossRot) return;
        else rigid.isKinematic = true;
        if (bossHealth.IsDead) return;

        Quaternion rot = Quaternion.LookRotation(target.position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, rot, speed * Time.deltaTime);

        if(isAttack) // 100 이상일 경우 유도탄만 발사. 추후 bossHealth.CurrentHealth > 100 && ) 추가
        {
            isAttack = false;
            StartCoroutine(ShootAtk());
        }

        /*else if(bossHealth.CurrentHealth <= 100 && isAttack) // 100 이하일 때는 점프도 같이 (추후 추가)
        {
            isAttack = false;
            
            int a = Random.Range(1, 3);
            if (a==1)
            {
                StartCoroutine(ShootAtk());
            }
            else if(a==2)
            {
                StartCoroutine(JumpAtk());
            }
        }*/
    }

    /*
    private IEnumerator JumpAtk()
    {
        yield return null;
    }
    */

    private IEnumerator ShootAtk()
    {
        animator.SetAtk(true);

        Instantiate(atkBulletPref, pos1);
        Instantiate(atkBulletPref, pos2);

        yield return new WaitForSeconds(2f);

        animator.SetAtk(false);

        yield return new WaitForSeconds(3f);

        yield return new WaitForSeconds(nextAtkTime);
        isAttack = true;
    }
}
