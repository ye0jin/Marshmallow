using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class PlayerOneShotWeapon : MonoBehaviour
{
    private PlayerMovement _playerMovement;
    private PlayerAnimator _playerAnimator;
    private PlayerInput _playerInput;
    
    public bool _isShoot = false;
    private bool _isReloading = false;
    public bool IsReloading => _isReloading;

    [SerializeField] private float _speed = 50f;
    [SerializeField] private int _maxAmmo = 50;
    public int MaxAmmo => _maxAmmo;

    private int _currentAmmo = 0;
    public int CurrentAmmo => _currentAmmo;
    
    [SerializeField] private GameObject _bullet;
    [SerializeField] private Transform _bulletPos;
    [SerializeField] private GameObject _bulletCase;
    [SerializeField] private Transform _casePos;

    [SerializeField] private ReloadGauge _reloadUI = null;

    private void Awake()
    {
        _playerAnimator = GameObject.Find("Player/Visual").GetComponent<PlayerAnimator>();
        _playerMovement = GameObject.Find("Player").GetComponent<PlayerMovement>();
        _playerInput = GameObject.Find("Player").GetComponent<PlayerInput>();


        _currentAmmo = _maxAmmo;
        _reloadUI.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && !_isShoot && !_playerInput.IsRolling && !_isReloading)
        {
            if (_currentAmmo <= 0)
            {
                SoundManager.Instance.PlayNoAmmo();
                //Debug.Log("총알없음");
                return;
            }
            SoundManager.Instance.PlayOneShot();
            _isShoot = true;
            
            --_currentAmmo;
            StartCoroutine(StartShooting());
        }

        if(Input.GetKeyDown(KeyCode.R) && !_playerInput.IsRolling && !_isReloading)
        {
            _isReloading = true;
            //Debug.Log("재장전");
            ReloadAmmo();
        }
    }

    private void ReloadAmmo()
    {
        _isShoot = true;
        _playerAnimator?.SetReload(true); // 애니메이션 설정

        Reload();
    }

    public void Reload()
    {
        StartCoroutine(ReloadCoroutine());
    }

    private IEnumerator ReloadCoroutine()
    {
        _reloadUI.gameObject.SetActive(true);
        float time = 0;

        while (time <= 3.0f)
        {
            time += Time.deltaTime;
            _reloadUI.ReloadGaugeNormal(time / 3.0f);
            yield return null;
        }

        _reloadUI.gameObject.SetActive(false);

        _currentAmmo = _maxAmmo;

        SoundManager.Instance.PlayReloadSound();
        _playerAnimator?.SetReload(false);
        _isShoot = false;
        _isReloading = false;
    }

    private IEnumerator StartShooting()
    {
        _playerAnimator?.SetShoot(_isShoot);

        // 터치한 방향 찾기
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        Vector3 targetDirection;

        if (Physics.Raycast(ray, out hit))
        {
            targetDirection = hit.point - _bulletPos.position;
            targetDirection.y = 0f;

            Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
            _playerInput.SetRotation(targetRotation);
        }
        else
        {
            targetDirection = _bulletPos.forward;
        }

        yield return new WaitForSeconds(0.2f);

       // 총알 발사
        GameObject prefabBullet = Instantiate(_bullet, _bulletPos.position, _bulletPos.rotation);
        Rigidbody b_rigid = prefabBullet.GetComponent<Rigidbody>();
        b_rigid.velocity = targetDirection.normalized * _speed;

        // 탄피 배출
        GameObject prefabCase = Instantiate(_bulletCase, _casePos.position, _casePos.rotation);
        Rigidbody c_rigid = prefabCase.GetComponent<Rigidbody>();
        Vector3 caseVec = _casePos.forward * Random.Range(-3, -2) + Vector3.up * Random.Range(2, 3);

        c_rigid.AddForce(caseVec, ForceMode.Impulse);
        c_rigid.AddTorque(Vector3.up * 10, ForceMode.Impulse);

        _playerAnimator?.SetShoot(false); 
        yield return new WaitForSeconds(0.4f);

        //
        Destroy(prefabCase, 5);
        _isShoot = false;
    }
}
