using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Bullet : MonoBehaviour
{
    private WeaponManager _weaponManager;
    private EnemyController _enemyController;
    private EnemyHealth _enemyHealth;
    private HealthGauge _healthGauge;

    private BossHealth bossHealth;

    //private Material _mat;


    private void Awake()
    {
        _weaponManager = GameObject.Find("Player/Visual/Bone_Body/Bone_Shoulder_R/Bone_Arm_R/RightHand/Weapon").GetComponent<WeaponManager>();
    }

    private void Start()
    {
        Destroy(gameObject, 10f); // 10초 후에는 무조건 사라짐
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag=="Map")
        {
            Destroy(gameObject);
        }
        else if(collision.gameObject.tag=="Enemy")
        {
            Destroy(gameObject);
            SoundManager.Instance.PlayEnemyAttackSound();

            GameObject enemyObject = collision.gameObject;
            _enemyController = enemyObject.GetComponent<EnemyController>();
            _enemyHealth = enemyObject.GetComponent<EnemyHealth>();
            _healthGauge = enemyObject.transform.Find("Visual/Bone_Body/Body/HealthGauge").GetComponent<HealthGauge>();

            //Debug.Log("enemy hurt");

            float dmg = _enemyHealth.MaxHealth;

            if (_weaponManager.CurrentWeaponState == WeaponState.Oneshot)
            {
                int weaponDam = 10;
                if (UIManager.Instance.BuyDoubleDamage) weaponDam = weaponDam * 2;
                Debug.Log(weaponDam);

                _enemyHealth.OnDamage(weaponDam);
                _healthGauge.DamageCheck(weaponDam / dmg);
            }
            else if(_weaponManager.CurrentWeaponState==WeaponState.Machine)
            {
                int weaponDam = 1;
                if (UIManager.Instance.BuyDoubleDamage) weaponDam = weaponDam * 2;
                Debug.Log(weaponDam);

                _enemyHealth.OnDamage(weaponDam);
                _healthGauge.DamageCheck(weaponDam / dmg);
            }
            //StartCoroutine(OnDamge());
        }
        else if(collision.gameObject.tag == "Boss")
        {
            Destroy(gameObject);
            GameObject bossObj = collision.gameObject;
            
            bossObj.GetComponent<Boss>();
            bossHealth = bossObj.GetComponent<BossHealth>();

            if (_weaponManager.CurrentWeaponState == WeaponState.Oneshot)
            {
                int weaponDam = 10;
                if (UIManager.Instance.BuyDoubleDamage) weaponDam *= 2;
                bossHealth.OnDamage(weaponDam);
            }
            else if (_weaponManager.CurrentWeaponState == WeaponState.Machine)
            {
                int weaponDam = 1;
                if (UIManager.Instance.BuyDoubleDamage) weaponDam *= 2;
                bossHealth.OnDamage(weaponDam);
            }
        }
        else if (collision.gameObject.tag == "BossBullet")
        {
            Destroy(gameObject);
            collision.gameObject.GetComponent<FollowTargetBullet>().Explode();
            collision.gameObject.transform.GetComponentInChildren<DeadFeedback>().Drop();
        }
    }
}
